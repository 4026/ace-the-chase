using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Increase max speed, but also pursuit speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/The Thrill", fileName = "Player_TheThrill")]
    public class TheThrill : PlayerCard
    {
        public int MaxSpeedIncrease;
        public int PursuitSpeedIncrease;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddMaxSpeed(MaxSpeedIncrease)
                .AddPursuitSpeed(PursuitSpeedIncrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}