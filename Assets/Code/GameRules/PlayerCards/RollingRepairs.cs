using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Perform a repair in exchange for control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Rolling Repairs", fileName = "Player_RollingRepairs")]
    public class RollingRepairs : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Repair;
        
        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Rolling Repairs is a repair, so it requires a Damage card as a parameter.
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
            ChaseMutator mutator = new ChaseMutator(currentState, uiManager)
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
                .DiscardFromHand(this)
                .Done();
        }
    }
}