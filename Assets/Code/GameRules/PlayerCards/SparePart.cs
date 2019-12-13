using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Perform a cheap repair, but exhaust this card to do it.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Spare Part", fileName = "Player_SparePart")]
    public class SparePart : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Repair;
        
        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Spare Part is a repair, so it requires a Damage card as a parameter.
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
            ChaseMutator mutator = new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this);

            foreach (IPlayerCard repairedDamage in targetCards)
            {
                if (repairedDamage != null)
                {
                    mutator.ExhaustFromHand(repairedDamage);
                }
            }

            return mutator
                .ExhaustFromHand(this)
                .Done();
        }
    }
}