using System;
using System.Collections.Generic;
using AceTheChase.UI;

namespace AceTheChase.GameRules
{
    public class RouteCardParameterProvider : IProvidesCardParameters
    {
        private Action<IDictionary<string, object>> OnComplete;

        public void PromptForParameters(
            Chase currentChaseState,
            UIManager uiManager,
            Action<IDictionary<string, object>> OnComplete
        )
        {
            uiManager.DisplayCardPicker(currentChaseState.CurrentRoute);
            uiManager.RouteCardClicked += this.CardSelected;
            this.OnComplete = OnComplete;
        }

        private void CardSelected(IRouteCard card)
        {
            this.OnComplete(new Dictionary<string, object>() {
                { "routeCard", card }
            });
        }
    }
}