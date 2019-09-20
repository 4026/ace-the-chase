using System.Collections.Generic;
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

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddLead(LeadGain)
                .AddDamageToTopOfDeck(DamageCardsGiven)
                .DiscardFromHand(this)
                .Done();
        }
    }
}