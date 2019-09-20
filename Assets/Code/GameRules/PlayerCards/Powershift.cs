using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Gain both speed and control, provided you have control to spare.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Powershift", fileName = "Player_Powershift")]
    public class Powershift : PlayerCard
    {
        public int SpeedIncrease;
        public int ControlIncrease;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, List<ICard>> additionalParameters,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddControl(ControlIncrease)
                .AddPlayerSpeed(SpeedIncrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}