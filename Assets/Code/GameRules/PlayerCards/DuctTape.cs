using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Discard a damage card (instead of trashing it).
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Duct Tape", fileName = "Player_DuctTape")]
    public class DuctTape : PlayerCard
    { 
        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Duct Tape targets a Damage card in the player's hand.
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
                    mutator.DiscardFromHand(repairedDamage);
                }
            }

            return mutator
                .DiscardFromHand(this)
                .Done();
        }
    }
}