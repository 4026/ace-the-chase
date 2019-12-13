using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Reduce pursuit speed, at the cost of your own speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Hide", fileName = "Player_Hide")]
    public class Hide : PlayerCard
    {
        public int PursuitSpeedDecrease;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddPursuitSpeed(-1 * PursuitSpeedDecrease)
                .AddPlayerSpeed(-1 * currentState.PlayerSpeed)
                .DiscardFromHand(this)
                .Done();
        }
    }
}