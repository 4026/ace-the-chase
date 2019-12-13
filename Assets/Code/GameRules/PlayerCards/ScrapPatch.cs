using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Repair; resurrect the last card that went into the trash.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Scrap Patch", fileName = "Player_ScrapPatch")]
    public class ScrapPatch : PlayerCard
    {       
        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Capacious Toolbox is a repair, so needs to target a Damage card in the player's hand.
            return new CardParameterProvider<IPlayerCard>(
                chaseState.Hand
                    .Where(card => card.CardType == PlayerCardType.Damage)
                    .ToList()
            );
        }

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            int cardsShuffled = currentState.PlayerDiscard.Count;

            ChaseMutator mutator = new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this);

            if (currentState.PlayerExhaust.Count > 0)
            {
                mutator.ResurrectFromExhaustToHand(currentState.PlayerExhaust.Last());
            }

            foreach (IPlayerCard repairedDamage in targetCards)
            {
                if (repairedDamage != null)
                {
                    mutator.ExhaustFromHand(repairedDamage);
                }
            }

            return mutator
                .DiscardFromHand(this)
                .Done();
        }
    }
}