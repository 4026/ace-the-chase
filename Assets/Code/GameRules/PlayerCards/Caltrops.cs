using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// A simple card that adds speed in exchange for Control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Caltrops", fileName = "Player_Caltrops")]
    public class Caltrops : PlayerCard
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
                .AddPursuitSpeed(-PursuitSpeedDecrease)
                .ExhaustFromHand(this)
                .Done();
        }
    }
}