using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Increases everyone's speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Expressway", fileName = "Routes_Expressway")]
    public class Expressway : RouteCard
    {
        public int SpeedIncrease;
        public int PursuitSpeedIncrease;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            return new ChaseMutator(currentState, uiManager)
                .AddPlayerSpeed(SpeedIncrease)
                .AddPursuitSpeed(PursuitSpeedIncrease)
                .DiscardFromRoute(this)
                .Done();
        }
    }
}