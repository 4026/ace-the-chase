using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Reduces control next turn.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Shopping Mall", fileName = "Routes_ShoppingMall")]
    public class ShoppingMall : RouteCard
    {
        public int ControlDecrease;

        public override Chase Play(Chase currentState, UIManager uiManager)
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .ActivateCard(this)
                .AddControl(-ControlDecrease)
                .DiscardFromRoute(this)
                .Done();
        }
    }
}