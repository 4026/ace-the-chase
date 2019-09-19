using System.Collections.Generic;
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
            IDictionary<string, object> additionalParameters
        )
        {
            return new ChaseMutator(currentState)
                .RecycleRouteDeck()
                .AddControl(-this.ControlCost)
                .DiscardFromHand(this)
                .Done();
        }
    }
}