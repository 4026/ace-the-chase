using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Perform a diversion in exchange for control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Shortcut", fileName = "Player_Shortcut")]
    public class Shortcut : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Diversion;
        
        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters,
            UIManager uiManager
        )
        {
            IRouteCard discardedRouteCard = additionalParameters["discardedRouteCard"] as IRouteCard;

            return new ChaseMutator(currentState, uiManager)
                .DiscardFromRoute(discardedRouteCard)
                .AddControl(-this.ControlCost)
                .DiscardFromHand(this)
                .Done();
        }
    }
}