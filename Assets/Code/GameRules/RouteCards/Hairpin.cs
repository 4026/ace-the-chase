using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Damages the car unless the player's speed is low.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Hairpin", fileName = "Routes_Hairpin")]
    public class Hairpin : RouteCard
    {
        public int Damage;
        public int SpeedThreshold;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            ChaseMutator mutator = new ChaseMutator(currentState, uiManager);
            if (currentState.PlayerSpeed > SpeedThreshold)
            {
                mutator.AddDamageToTopOfDeck(Damage);
            }

            return mutator.DiscardFromRoute(this).Done();
        }
    }
}