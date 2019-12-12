using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Take damage at high speeds
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Cobbled Street", fileName = "Routes_CobbledStreet")]
    public class CobbledStreet : RouteCard
    {
        public int SpeedThreshold;
        public int Damage;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            ChaseMutator mutator = new ChaseMutator(currentState, uiManager, $"playing {this.Name}");
            mutator.ActivateCard(this);
            if (currentState.PlayerSpeed > SpeedThreshold)
            {
                mutator.AddDamageToTopOfDeck(Damage);
            }
            return mutator.DiscardFromRoute(this).Done();
        }
    }
}