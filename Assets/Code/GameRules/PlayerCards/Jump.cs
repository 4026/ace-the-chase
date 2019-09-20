using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Trade speed for control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Jump", fileName = "Player_Jump")]
    public class Jump : PlayerCard
    {
        public int CardsDrawn;
        public int DamageAdded;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .DrawCards(3)
                .AddDamageToTopOfDeck(1)
                .DiscardFromHand(this)
                .Done();
        }
    }
}