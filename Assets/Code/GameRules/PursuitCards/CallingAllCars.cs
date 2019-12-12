using AceTheChase.UI;
using UnityEngine;
namespace AceTheChase.GameRules.PursuitCards
{
    /// <summary>
    /// Increases the pursuit speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Pursuit/Calling All Cars", fileName = "Pursuit_CallingAllCars")]
    public class CallingAllCars : PursuitCard
    {
        public int PursuitSpeedIncrease;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .ActivateCard(this)
                .AddPursuitSpeed(PursuitSpeedIncrease)
                .Done();
        }
    }
}