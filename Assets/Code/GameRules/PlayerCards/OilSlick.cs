using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Increase lead.false More effective if played on a maneuver.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/OilSlick", fileName = "Player_OilSlick")]
    public class OilSlick : PlayerCard
    {
        public int LeadIncrease;
        public int LeadIncreaseOnManeuver;
        
        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            ChaseMutator mutator = new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddLead(LeadIncrease);

            if (currentState.CurrentRoute.Exists(route => route.CardType == RouteCardType.Maneuver))
            {
                mutator.AddLead(LeadIncreaseOnManeuver);
            }

            return mutator
                .ExhaustFromHand(this)
                .Done();
        }
    }
}