using System.Collections.Generic;
using AceTheChase.UI;
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
            IDictionary<string, object> additionalParameters,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager)
                .AddControl(
                    Mathf.FloorToInt(ControlPerSpeed * currentState.PlayerSpeed) - this.ControlCost
                )
                .DiscardFromHand(this)
                .Done();
        }
    }
}