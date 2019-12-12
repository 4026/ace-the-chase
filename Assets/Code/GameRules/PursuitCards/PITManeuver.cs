using AceTheChase.UI;
using UnityEngine;
namespace AceTheChase.GameRules.PursuitCards
{
    /// <summary>
    /// Reduces the player's control next turn
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Pursuit/Pit Maneuver", fileName ="Pursuit_PitManeuver")]
    public class PITManeuver : PursuitCard
    {
        public int ControlDecrease;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .ActivateCard(this)
                .AddControl(-ControlDecrease)
                .Done();
        }
    }
}