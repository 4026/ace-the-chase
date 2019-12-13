using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Stunt; draw cards, take damage.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Jump", fileName = "Player_Jump")]
    public class Jump : PlayerCard
    {
        public int CardsDrawn;
        public int DamageAdded;

        public override PlayerCardType CardType => PlayerCardType.Stunt;

        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Jump is a stunt, so it requires a Maneuver card as a parameter.
            return new CardParameterProvider<IRouteCard>(
                chaseState.CurrentRoute
                    .Where(card => card.CardType == RouteCardType.Maneuver)
                    .ToList()
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

            foreach (IRouteCard targetCard in targetCards)
            {
                if (targetCard != null)
                {
                    mutator.DiscardFromRoute(targetCard);
                }
            }

            return mutator
                .DrawCards(CardsDrawn)
                .AddDamageToTopOfDeck(DamageAdded)
                .DiscardFromHand(this)
                .Done();
        }
    }
}