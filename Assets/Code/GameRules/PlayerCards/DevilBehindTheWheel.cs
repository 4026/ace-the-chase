using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Risk higher pursuit for 1 big turn
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/DevilBehindTheWheel", fileName = "Player_DevilBehindTheWheel")]
    public class DevilBehindTheWheel : PlayerCard
    {
        public int InitialPursuitIncrease;
        public int ControlIncrease;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddPursuitSpeed(InitialPursuitIncrease)
                .DrawCards(currentState.PursuitSpeed)
                .AddControl(ControlIncrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}