using System.Collections.Generic;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Trade speed for control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Brake", fileName = "Player_Brake")]
    public class Brake : PlayerCard
    {
        public int SpeedDecrease;
        public int ControlIncrease;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters
        )
        {
            return new ChaseMutator(currentState)
                .AddPlayerSpeed(-SpeedDecrease)
                .AddControl(ControlIncrease - this.ControlCost)
                .DiscardFromHand(this)
                .Done();
        }
    }
}