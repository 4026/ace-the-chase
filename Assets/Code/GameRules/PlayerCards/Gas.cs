using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// A simple card that adds speed in exchange for Control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Gas", fileName = "Player_Gas")]
    public class Gas : PlayerCard
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