using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Decreases player's speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Heavy Traffic", fileName = "Routes_HeavyTraffic")]
    public class HeavyTraffic : RouteCard
    {
        public int SpeedDecrease;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            return new ChaseMutator(currentState, uiManager)
                .AddPlayerSpeed(-SpeedDecrease)
                .DiscardFromRoute(this)
                .Done();
        }
    }
}