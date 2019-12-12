using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Trade speed for control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Brake", fileName = "Player_Brake")]
    public class Brake : PlayerCard
    {
        public int SpeedDecrease;
        public int ControlIncrease;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddControl(ControlIncrease)
                .AddPlayerSpeed(-SpeedDecrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}