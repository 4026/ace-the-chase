using UnityEngine;

namespace AceTheChase.GameRules.PursuitCards
{
    /// <summary>
    /// Increases the pursuit speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Pursuit/Police Helicopter", fileName = "Pursuit_PoliceHelicopter")]
    public class PoliceHelicopter : PursuitCard
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