using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Reduces lead, and damages the car unless the player's speed is high.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Raised Bridge", fileName = "Routes_RaisedBridge")]
    public class RaisedBridge : RouteCard
    {
        public int LeadDecrease;
        public int SpeedThreshold;
        public int Damage;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            ChaseMutator mutator = new ChaseMutator(currentState, uiManager, $"playing {this.Name}");
            mutator.ActivateCard(this);
            mutator.AddLead(-LeadDecrease);
            if (currentState.PlayerSpeed < SpeedThreshold)
            {
                mutator.AddDamageToTopOfDeck(Damage);
            }

            return mutator.DiscardFromRoute(this).Done();
        }
    }
}