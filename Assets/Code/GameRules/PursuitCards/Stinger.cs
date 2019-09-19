using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PursuitCards
{
    /// <summary>
    /// Add damage to the top of the player's deck.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Pursuit/Stinger", fileName = "Pursuit_Stinger")]
    public class Stinger : PursuitCard
    {
        public int Damage;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            return new ChaseMutator(currentState, uiManager)
                .AddDamageToTopOfDeck(Damage)
                .Done();
        }
    }
}