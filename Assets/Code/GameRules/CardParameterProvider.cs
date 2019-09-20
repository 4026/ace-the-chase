using System;
using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules
{
    public class CardParameterProvider<TCard> : IProvidesCardParameters where TCard : ICard
    {
        private string parameterName;
        private IList<TCard> candidateCards;
        private int numCards;
        private List<ICard> selectedCards;

        private Action<IDictionary<string, List<ICard>>> OnComplete;

        private UIManager uiManager;

        public CardParameterProvider(string parameterName, IList<TCard> candidateCards, int numCards = 1)
        {
            this.selectedCards = new List<ICard>();
            this.parameterName = parameterName;
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
            Action<IDictionary<string, List<ICard>>> OnComplete,
            Action OnCancel
        )
        {
            this.uiManager = uiManager;
            
            uiManager.DisplayCardPicker(candidateCards);
            uiManager.CardClicked += this.CardSelected;
            uiManager.CardPickerCancelled += () => { Cancel(OnCancel); };
            uiManager.CardPickerNoTarget += () => { CardSelected(null); };

            this.OnComplete = OnComplete;
        }

        private void CardSelected(UICardView card)
        {
            if (card != null)
            {
                ICard cardClicked = card.GetCard();
                selectedCards.Add(cardClicked);
                candidateCards.Remove((TCard)cardClicked);
            }
            if (selectedCards.Count >= numCards
                || candidateCards.Count == 0)
            {
                this.uiManager.CardClicked -= this.CardSelected;
                this.uiManager.HideCardPicker();
                this.OnComplete(
                    new Dictionary<string, List<ICard>> {
                    { parameterName, selectedCards }
                });
            }
            else if (card != null)
            {
                UnityEngine.Object.Destroy(card.gameObject);
            }
        }
    }
}