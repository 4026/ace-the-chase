using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Gain lots of speed at the cost of control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/CarSwap", fileName = "Player_CarSwap")]
    public class CarSwap : PlayerCard
    {
        public RouteCard CarSwapCard;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddCardToRouteDeck(CarSwapCard)
                .DiscardFromHand(this)
                .Done();
        }
    }
}