using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Recycle the route deck.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Double Back", fileName = "Player_DoubleBack")]
    public class DoubleBack : PlayerCard
    {
        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .RecycleRouteDeck()
                .DiscardFromHand(this)
                .Done();
        }
    }
}