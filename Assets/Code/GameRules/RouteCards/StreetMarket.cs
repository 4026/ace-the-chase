using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Reduces control next turn based on current speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Street Market", fileName = "Routes_StreetMarket")]
    public class StreetMarket : RouteCard
    {
        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .ActivateCard(this)
                .AddControl(-currentState.PlayerSpeed)
                .DiscardFromRoute(this)
                .Done();
        }
    }
}