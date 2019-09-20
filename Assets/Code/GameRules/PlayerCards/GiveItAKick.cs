using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Recycle the route deck.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Give It A Kick", fileName = "Player_GiveItAKick")]
    public class GiveItAKick : PlayerCard
    {
        public int CardsToDiscard;
        public int CardsToDraw;

        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            List<IPlayerCard> selection = chaseState.Hand;
            selection.Remove((IPlayerCard)this);
            // Drift is a stunt, so it requires a Maneuver card as a parameter.
            CardParameterProvider <IPlayerCard> cards =  new CardParameterProvider<IPlayerCard>(
                "discards",
                selection,
                2
            );
            return cards;
        }

        public override Chase Play(
            Chase currentState,
            IDictionary<string, List<ICard>> additionalParameters,
            UIManager uiManager
        )
        {
            IPlayerCard discardedCard1 = null;
            IPlayerCard discardedCard2 = null;
            if (additionalParameters.ContainsKey("discards"))
            {
                if (additionalParameters["discards"].Count > 0
                    && additionalParameters["discards"][0] != null)
                {
                    discardedCard1 = additionalParameters["discards"][0] as IPlayerCard;
                }
                if (additionalParameters["discards"].Count > 1
                    && additionalParameters["discards"][1] != null)
                {
                    discardedCard2 = additionalParameters["discards"][1] as IPlayerCard;
                }
            }
            ChaseMutator chase = new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .DrawCards(CardsToDraw);
            if (discardedCard1 != null)
            {
                chase.DiscardFromHand(discardedCard1);
            }
            if (discardedCard2 != null)
            {
                chase.DiscardFromHand(discardedCard2);
            }
            return chase.DiscardFromHand(this)
                    .Done();
        }
    }
}