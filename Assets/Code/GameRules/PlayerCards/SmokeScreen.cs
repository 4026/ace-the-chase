using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Add lead; add more lead if your lead is low.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/SmokeScreen", fileName = "Player_SmokeScreen")]
    public class SmokeScreen : PlayerCard
    {
        public int LeadGained;
        public int ExtraLeadGainMaxTrigger;
        public int ExtraLeadGainValue;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager )
        {
            ChaseMutator chaseMutator = new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddLead(LeadGained);

            if (currentState.Lead <= ExtraLeadGainMaxTrigger)
            {
                chaseMutator.AddLead(ExtraLeadGainValue);
            }

            return chaseMutator
                .ExhaustFromHand(this)
                .Done();
        }
    }
}