using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Trade speed for control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/InTheZone", fileName = "Player_InTheZone")]
    public class InTheZone : PlayerCard
    {
        public int SpeedThreshold;
        public int ControlIncrease;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            var controlToIncrease = ControlIncrease;

            if(currentState.PlayerSpeed < SpeedThreshold)
            {
                controlToIncrease = 0;
            }
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddControl(controlToIncrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}