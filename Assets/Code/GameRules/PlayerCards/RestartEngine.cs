using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Recycle deck, drawing more cards if you've cycled a lot of cards.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Restart Engine", fileName = "Player_RestartEngine")]
    public class RestartEngine : PlayerCard
    {
        public int MinCardsShuffledForDraw;
        public int NumCardsToDraw;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            int cardsShuffled = currentState.PlayerDiscard.Count;

            ChaseMutator mutator = new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .RecyclePlayerDeck();

            if (cardsShuffled >= MinCardsShuffledForDraw)
            {
                mutator.DrawCards(NumCardsToDraw);
            }

            return mutator
                .ExhaustFromHand(this)
                .Done();
        }
    }
}