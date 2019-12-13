using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Draw 2 out of 3 cards from the top of the player's deck.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/The Plan", fileName = "Player_ThePlan")]
    public class ThePlan : PlayerCard
    {
        public int NumCardsToPeek;
        public int NumCardsToDraw;

        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // The Plan needs to know which of the top 3 cards on the player's deck the player 
            // wants to draw.
            return new CardParameterProvider<IPlayerCard>(
                chaseState.PlayerDeck.Peek(NumCardsToPeek),
                NumCardsToDraw
            );
        }

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            ChaseMutator mutator = new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this);
                
            foreach(IPlayerCard card in targetCards)
            {
                mutator.DrawSpecific(card);
            }

            return mutator
                .DiscardFromHand(this)
                .Done();
        }
    }
}