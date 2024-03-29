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
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddControl(ControlIncrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}