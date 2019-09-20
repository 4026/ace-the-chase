using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;
using System.Linq;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Gain control based on your current speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Nerves Of Steel", fileName = "Player_NervesOfSteel")]
    public class NervesOfSteel : PlayerCard
    {
        public float ControlPerSpeed;

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
                .AddControl(
                    -this.ControlCost
                )
                .ActivateCard(this)
                .AddControl(
                    Mathf.FloorToInt(ControlPerSpeed * currentState.PlayerSpeed)
                )
                .DiscardFromHand(this);


            if (additionalParameters.ContainsKey("maneuver"))
            {
                IRouteCard discardedRouteCard = additionalParameters["maneuver"] as IRouteCard;
                if(discardedRouteCard != null)
                muta.DiscardFromRoute(discardedRouteCard);
            }

            return muta.Done();
        }
    }
}