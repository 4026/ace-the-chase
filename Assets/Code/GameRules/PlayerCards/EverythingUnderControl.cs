using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Diversion; Gain lead, but take damage.
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
                chaseState.CurrentRoute
                    .Where(card => card.CardType == RouteCardType.Obstacle)
                    .ToList()
            );
        }

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            ChaseMutator muta = new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddLead(LeadGain)
                .AddDamageToTopOfDeck(DamageCardsGiven);

            foreach (IRouteCard targetCard in targetCards)
            {
                if (targetCard != null)
                {
                    muta.DiscardFromRoute(targetCard);
                }
            }

            return muta
                .DiscardFromHand(this)
                .Done();
        }
    }
}