using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Gain control, provided you have control to spare.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Everythings Under Control", fileName = "Player_EverythingsUnderControl")]
    public class EverythingUnderControl : PlayerCard
    {
        public int DamageCardsGiven;
        public int LeadGain;

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
            IDictionary<string, List<ICard>> additionalParameters,
            UIManager uiManager
        )
        {
            ChaseMutator muta = new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddLead(LeadGain)
                .AddDamageToTopOfDeck(DamageCardsGiven);
            if (additionalParameters.ContainsKey("manuver"))
            {
                IRouteCard discardedRouteCard = additionalParameters["maneuver"] as IRouteCard;
                if (discardedRouteCard != null)
                    muta.DiscardFromRoute(discardedRouteCard);
            }
            return muta.DiscardFromHand(this)
            .Done();
        }
    }
}