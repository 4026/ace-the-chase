using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// A simple card that adds speed in exchange for Control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/SmokeScreen", fileName = "Player_SmokeScreen")]
    public class SmokeScreen : PlayerCard
    {
        public int LeadGained;
        public int ExtraLeadGainMaxTrigger;
        public int ExtraLeadGainValue;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters,
            UIManager uiManager )
        {
            ChaseMutator chaseMutator = new ChaseMutator(currentState, uiManager)
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