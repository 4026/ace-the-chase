using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Reduces the player's speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Sharp Turn", fileName = "Routes_SharpTurn")]
    public class SharpTurn : RouteCard
    {
        public int SpeedDecrease;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .ActivateCard(this)
                .AddPlayerSpeed(-SpeedDecrease)
                .DiscardFromRoute(this)
                .Done();
        }
    }
}