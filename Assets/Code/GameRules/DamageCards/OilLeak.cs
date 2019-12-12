using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;


namespace AceTheChase.GameRules.DamageCards
{
    /// <summary>
    /// Damage. Reduce the player's lead.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Damage/Oil Leak", fileName = "Damage_OilLeak")]
    public class OilLeak : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Damage;
        
        public int LeadDecrease;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager)
                .ActivateCard(this)
                .AddLead(-this.LeadDecrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}