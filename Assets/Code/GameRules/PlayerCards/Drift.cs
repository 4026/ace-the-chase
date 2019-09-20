using System.Collections.Generic;
using System.Linq;
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
                   .DiscardFromHand(this);

            if (additionalParameters.ContainsKey("manuver")
                && additionalParameters["manuver"].Count > 0)
            {
                if (additionalParameters["maneuver"][0] != null)
                {
                    IRouteCard discardedRouteCard = additionalParameters["maneuver"][0] as IRouteCard;
                    if (discardedRouteCard != null)
                        muta.DiscardFromRoute(discardedRouteCard);
                }
            }


            return muta.Done();
        }
    }
}