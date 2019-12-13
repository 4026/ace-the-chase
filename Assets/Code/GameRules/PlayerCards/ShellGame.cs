using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Reduce the pursuit speed by the amount of control that the player has remaining.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Shell Game", fileName = "Player_ShellGame")]
    public class ShellGame : PlayerCard
    {
        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            int x = currentState.Control;

            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-x)
                .ActivateCard(this)
                .AddPursuitSpeed(-x)
                .DiscardFromHand(this)
                .Done();
        }
    }
}