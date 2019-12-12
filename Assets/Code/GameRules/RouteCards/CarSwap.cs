using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Damages the car unless the player's speed is low.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/CarSwap", fileName = "Routes_CarSwap")]
    public class CarSwap : RouteCard
    {
        public int SetSpeedTo;
        public int LeadIncrease;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            ChaseMutator mutator = new ChaseMutator(currentState, uiManager, $"playing {this.Name}");
            mutator.ActivateCard(this);

            mutator.AddPlayerSpeed(-(currentState.PlayerSpeed - SetSpeedTo));
            mutator.AddLead(LeadIncrease);

            return mutator.ExhaustFromRoute(this).Done();
        }
    }
}