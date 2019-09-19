using UnityEngine;

namespace AceTheChase.GameRules.PursuitCards
{
    /// <summary>
    /// Reduces the player's lead
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Pursuit/SuspectSighted", fileName = "Pursuit_SuspectSighted")]
    public class SuspectSighted : PursuitCard
    {
        public int LeadDecrease;

        public override Chase Play(Chase currentState)
        {
            return new ChaseMutator(currentState)
                .AddLead(-LeadDecrease)
                .Done();
        }
    }
}