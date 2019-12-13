using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Tutor a diversion from your deck.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/The Knowledge", fileName = "Player_TheKnowledge")]
    public class TheKnowledge : PlayerCard
    {

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .TutorCard(card => card.CardType == PlayerCardType.Diversion)
                .DiscardFromHand(this)
                .Done();
        }
    }
}