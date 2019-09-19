using System.Collections.Generic;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Gain both speed and control, provided you have control to spare.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Powershift", fileName = "Player_Powershift")]
    public class Powershift : PlayerCard
    {
        public int SpeedIncrease;
        public int ControlIncrease;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters
        )
        {
            return new ChaseMutator(currentState)
                .AddPlayerSpeed(SpeedIncrease)
                .AddControl(ControlIncrease - this.ControlCost)
                .DiscardFromHand(this)
                .Done();
        }
    }
}