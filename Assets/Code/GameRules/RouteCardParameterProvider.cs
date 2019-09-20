using System;
using System.Collections.Generic;
using AceTheChase.UI;

namespace AceTheChase.GameRules
{
    public class CardParameterProvider<TCard> : IProvidesCardParameters where TCard : ICard
    {
        private string parameterName;
        private IList<TCard> candidateCards;

        private Action<IDictionary<string, object>> OnComplete;

        private UIManager uiManager;

        public CardParameterProvider(string parameterName, IList<TCard> candidateCards)
        {
            this.parameterName = parameterName;
            this.candidateCards = candidateCards;
        }

        public void Cancel(Action OnCancel)
        {
            uiManager.CardClicked -= this.CardSelected;
            OnCancel();
        }


        public void PromptForParameters(
            Chase currentChaseState,
            UIManager uiManager,
            Action<IDictionary<string, object>> OnComplete,
            Action OnCancel
        )
        {
            this.uiManager = uiManager;
            
            uiManager.DisplayCardPicker(candidateCards);
            uiManager.CardClicked += this.CardSelected;
            uiManager.CardPickerCancelled += () => { Cancel(OnCancel); };

            this.OnComplete = OnComplete;
        }

        private void CardSelected(ICard card)
        {
            this.uiManager.HideCardPicker();
            uiManager.CardClicked -= this.CardSelected;
            this.OnComplete(new Dictionary<string, object>() {
                { parameterName, card }
            });
        }
    }
}