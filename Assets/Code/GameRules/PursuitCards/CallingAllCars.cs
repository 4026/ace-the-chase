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

        public override Chase Play(Chase currentState)
        {
            return new ChaseMutator(currentState)
                .AddPursuitSpeed(PursuitSpeedIncrease)
                .Done();
        }
    }
}