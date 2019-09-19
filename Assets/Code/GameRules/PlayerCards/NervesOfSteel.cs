using System.Collections.Generic;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Gain control based on your current speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Nerves Of Steel", fileName = "Player_NervesOfSteel")]
    public class NervesOfSteel : PlayerCard
    {
        public float ControlPerSpeed;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters
        )
        {
            return new ChaseMutator(currentState)
                .AddControl(
                    Mathf.FloorToInt(ControlPerSpeed * currentState.PlayerSpeed) - this.ControlCost
                )
                .DiscardFromHand(this)
                .Done();
        }
    }
}