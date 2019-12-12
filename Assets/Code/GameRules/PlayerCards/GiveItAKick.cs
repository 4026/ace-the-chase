using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Discard cards, then draw more.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Give It A Kick", fileName = "Player_GiveItAKick")]
    public class GiveItAKick : PlayerCard
    {
        public int CardsToDiscard;
        public int CardsToDraw;

        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Prompt the player to choose some cards from their hand that aren't this card.
            List<IPlayerCard> selection = chaseState.Hand;
            selection.Remove((IPlayerCard)this);
            CardParameterProvider <IPlayerCard> cards =  new CardParameterProvider<IPlayerCard>(
                selection,
                2
            );

            return cards;
        }

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            ChaseMutator chase = new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .DrawCards(CardsToDraw);

            foreach (IPlayerCard discardedCard in targetCards)
            {
                if (discardedCard != null)
                {
                    chase.DiscardFromHand(discardedCard);
                }
            }

            return chase
                .DiscardFromHand(this)
                .Done();
        }
    }
}