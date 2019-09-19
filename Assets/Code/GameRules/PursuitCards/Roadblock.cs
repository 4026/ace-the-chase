using UnityEngine;

namespace AceTheChase.GameRules.PursuitCards
{
    /// <summary>
    /// Reduces the player's speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Pursuit/Road Block", fileName = "Pursuit_RoadBlock")]
    public class Roadblock : PursuitCard
    {
        public int SpeedDecrease;

        public override Chase Play(Chase currentState)
        {
            return new ChaseMutator(currentState)
                .AddPlayerSpeed(-SpeedDecrease)
                .Done();
        }
    }
}