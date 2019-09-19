using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Reduces lead if the player's speed is high.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Speed Camera", fileName = "Routes_SpeedCamera")]
    public class SpeedCamera : RouteCard
    {
        public int SpeedThreshold;
        public int LeadDecrease;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            ChaseMutator mutator = new ChaseMutator(currentState, uiManager);
            if (currentState.PlayerSpeed > SpeedThreshold)
            {
                mutator.AddLead(-LeadDecrease);
            }

            return mutator.DiscardFromRoute(this).Done();
        }
    }
}