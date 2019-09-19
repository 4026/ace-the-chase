using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Gain control, provided you have control to spare.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Well Oiled Gearbox", fileName = "Player_WellOiledGearbox")]
    public class WellOiledGearbox : PlayerCard
    {
        public int ControlIncrease;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager)
                .AddControl(ControlIncrease - this.ControlCost)
                .DiscardFromHand(this)
                .Done();
        }
    }
}