using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Perform a stunt in exchange for control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Drift", fileName = "Player_Drift")]
    public class Drift : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Stunt;

        public override IProvidesCardParameters GetParameterProvider()
        {
            // Drift is a stunt, so it requires a route card as a parameter.
            return new RouteCardParameterProvider();
        }
        
        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters,
            UIManager uiManager
        )
        {
            IRouteCard discardedRouteCard = additionalParameters["routeCard"] as IRouteCard;

            return new ChaseMutator(currentState, uiManager)
                .DiscardFromRoute(discardedRouteCard)
                .AddControl(-this.ControlCost)
                .DiscardFromHand(this)
                .Done();
        }
    }
}