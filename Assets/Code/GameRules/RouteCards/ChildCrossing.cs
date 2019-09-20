using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Reduces lead, and damages the car unless the player's speed is high.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Child Crossing", fileName = "Routes_ChildCrossing")]
    public class ChildCrossing : RouteCard
    {
        public int SpeedDecrease;
        public int SpeedThreshold;
        public int PursuitSpeedIncrease;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            ChaseMutator mutator = new ChaseMutator(currentState, uiManager);
            mutator.ActivateCard(this);
            mutator.AddPlayerSpeed(-SpeedDecrease);
            if (currentState.PlayerSpeed > SpeedThreshold)
            {
                mutator.AddPursuitSpeed(PursuitSpeedIncrease);
            }

            return mutator.DiscardFromRoute(this).Done();
        }
    }
}