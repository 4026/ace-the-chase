using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Increase speed, reduce pursuit speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Hacked Traffic Signals", fileName = "Player_HackedTrafficSignals")]
    public class HackedTrafficSignals : PlayerCard
    {
        public int SpeedIncrease;
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
                .AddPlayerSpeed(SpeedIncrease)
                .AddPursuitSpeed(PursuitSpeedDecrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}