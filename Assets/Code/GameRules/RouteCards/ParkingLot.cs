using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Decreases everyone's speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Parking Lot", fileName = "Routes_ParkingLot")]
    public class ParkingLot : RouteCard
    {
        public int SpeedDecrease;
        public int PursuitSpeedDecrease;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .ActivateCard(this)
                .AddPlayerSpeed(-SpeedDecrease)
                .AddPursuitSpeed(-PursuitSpeedDecrease)
                .DiscardFromRoute(this)
                .Done();
        }
    }
}