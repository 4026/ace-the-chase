using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Increase Max Speed
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Fine Tuning", fileName = "Player_FineTuning")]
    public class FineTuning : PlayerCard
    {
        public int MaxSpeedIncrease;
        
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
                .ExhaustFromHand(this)
                .Done();
        }
    }
}