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

        public void PromptForParameters(
            Chase currentChaseState,
            UIManager uiManager,
            Action<IDictionary<string, object>> OnComplete,
            Action OnCancel
        )
        {
            this.uiManager = uiManager;
            
            uiManager.DisplayCardPicker(candidateCards);
            uiManager.RouteCardClicked += this.CardSelected;
            uiManager.CardPickerCancelled += OnCancel;

            this.OnComplete = OnComplete;
        }

        private void CardSelected(IRouteCard card)
        {
            this.uiManager.HideCardPicker();
            this.OnComplete(new Dictionary<string, object>() {
                { parameterName, card }
            });
        }
    }
}