using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;
using System.Linq;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Trade speed for control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/HandbrakeTurn", fileName = "Player_HandbrakeTurn")]
    public class HandbrakeTurn : PlayerCard
    {
        public int SpeedDecrease;
        public int LeadIncrease;

        public override PlayerCardType CardType => PlayerCardType.Stunt;

        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Drift is a stunt, so it requires a Maneuver card as a parameter.
            return new CardParameterProvider<IRouteCard>(
                "maneuver",
                chaseState.CurrentRoute
                    .Where(card => card.CardType == RouteCardType.Maneuver)
                    .ToList()
            );
        }

        public override Chase Play(
            Chase currentState,
            IDictionary<string, List<ICard>> additionalParameters,
            UIManager uiManager
        )
        {
            ChaseMutator muta = new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddPlayerSpeed(SpeedDecrease)
                .AddLead(LeadIncrease)
                .DiscardFromHand(this);
                
            if (additionalParameters.ContainsKey("manuver")
                && additionalParameters["maneuver"].Count > 0
                && additionalParameters["maneuver"][0] != null)
            {
                IRouteCard discardedRouteCard = additionalParameters["maneuver"][0] as IRouteCard;
                if (discardedRouteCard != null)
                    muta.DiscardFromRoute(discardedRouteCard);
            }

            return muta.Done();
        }
    }
}