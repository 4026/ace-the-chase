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
            // RollingRepairs is a repair, so it requires a Damage card as a parameter.
            return new CardParameterProvider<IPlayerCard>(
                "damage",
                chaseState.Hand
                    .Where(card => card.CardType == PlayerCardType.Damage)
                    .ToList()
            );
        }
        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters,
            UIManager uiManager
        )
        {
            IPlayerCard repairedDamage = additionalParameters["damage"] as IPlayerCard;

            return new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .ExhaustFromHand(repairedDamage)
                .DiscardFromHand(this)
                .Done();
        }
    }
}