using System.Collections.Generic;
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
        
        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters,
            UIManager uiManager
        )
        {
            IPlayerCard repairedDamage = additionalParameters["repairedDamage"] as IPlayerCard;

            return new ChaseMutator(currentState, uiManager)
                .ExhaustFromHand(repairedDamage)
                .AddControl(-this.ControlCost)
                .DiscardFromHand(this)
                .Done();
        }
    }
}