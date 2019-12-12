using System;
using System.Collections.Generic;
using AceTheChase.UI;

namespace AceTheChase.GameRules
{
    public class CardParameterProvider<TCard> : IProvidesCardParameters where TCard : ICard
    {
        private IList<TCard> candidateCards;
        private int numCards;
        private List<ICard> selectedCards;

        private Action<List<ICard>> OnComplete;

        private UIManager uiManager;

        public CardParameterProvider(IList<TCard> candidateCards, int numCards = 1)
        {
            this.selectedCards = new List<ICard>();
            this.candidateCards = candidateCards;
            this.numCards = numCards;
        }

        public void Cancel(Action OnCancel)
        {
            uiManager.CardClicked -= this.CardSelected;
            OnCancel();
        }


        public void PromptForParameters(
            Chase currentChaseState,
            UIManager uiManager,
            Action<List<ICard>> OnComplete,
            Action OnCancel
        )
        {
            this.uiManager = uiManager;

            // If there are no available cards to choose from, just return an empty list
            // immediately.
            if (this.candidateCards.Count == 0)
            {
                OnComplete(new List<ICard>());
                return;
            }
            
            uiManager.DisplayCardPicker(candidateCards);
            uiManager.CardClicked += this.CardSelected;
            uiManager.CardPickerCancelled += () => { Cancel(OnCancel); };

            this.OnComplete = OnComplete;
        }

        private void CardSelected(UICardView card)
        {
            ICard cardClicked = card.GetCard();
            selectedCards.Add(cardClicked);
            candidateCards.Remove((TCard)cardClicked);

            if (selectedCards.Count >= numCards || candidateCards.Count == 0)
            {
                // If the player has selected the required number of cards or has exhausted all 
                // available candidate cards, we're done: return the list of selected cards.
                this.uiManager.CardClicked -= this.CardSelected;
                this.uiManager.HideCardPicker();
                this.OnComplete(selectedCards);
            }
            else if (card != null)
            {
                // Otherwise, just remove the selected card from the displayed list and let the
                // player keep picking.
                UnityEngine.Object.Destroy(card.gameObject);
            }
        }
    }
}