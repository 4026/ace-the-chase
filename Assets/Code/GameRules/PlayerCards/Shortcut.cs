using System.Collections.Generic;
using System.Linq;
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
        
        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Shortcut is a diversion, so it requires an Obstacle card as a parameter.
            return new CardParameterProvider<IRouteCard>(
                "obstacle",
                chaseState.CurrentRoute
                    .Where(card => card.CardType == RouteCardType.Obstacle)
                    .ToList()
            );
        }

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters,
            UIManager uiManager
        )
        {
            IRouteCard discardedRouteCard = additionalParameters["obstacle"] as IRouteCard;

            return new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlCost)
                .DiscardFromRoute(discardedRouteCard)
                .DiscardFromHand(this)
                .Done();
        }
    }
}