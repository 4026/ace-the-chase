using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Damages the car unless the player's speed is low.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Chicane", fileName = "Routes_Chicane")]
    public class Chicane : RouteCard
    {
        public int SpeedThreshold1;
        public int SpeedDecrease1;
        public int SpeedThreshold2;
        public int SpeedDecrease2;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            ChaseMutator mutator = new ChaseMutator(currentState, uiManager, $"playing {this.Name}");
            mutator.ActivateCard(this);
            bool triggerFirstDecrease = currentState.PlayerSpeed > SpeedThreshold1;
            bool triggerSecondDecrease = currentState.PlayerSpeed - SpeedDecrease1 > SpeedThreshold2;

            if (triggerFirstDecrease)
            {
                mutator.AddPlayerSpeed(-SpeedDecrease1);
            }
            if (triggerSecondDecrease)
            {
                mutator.AddPlayerSpeed(-SpeedDecrease2);
            }

            return mutator.DiscardFromRoute(this).Done();
        }
    }
}