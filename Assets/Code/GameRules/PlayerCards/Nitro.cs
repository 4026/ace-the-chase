using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Gain lots of speed at the cost of control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Nitro", fileName = "Player_Nitro")]
    public class Nitro : PlayerCard
    {
        public int SpeedIncrease;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddPlayerSpeed(SpeedIncrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}