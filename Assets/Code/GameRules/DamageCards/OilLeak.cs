using System.Collections.Generic;
using UnityEngine;


namespace AceTheChase.GameRules.DamageCards
{
    /// <summary>
    /// Damage. Reduce the player's lead.
    /// </summary
    [CreateAssetMenu(menuName = "Cards/Damage/Oil Leak", fileName = "Damage_OilLeak")]
    public class OilLeak : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Damage;
        
        public int LeadDecrease;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters
        )
        {
            return new ChaseMutator(currentState)
                .AddLead(-this.LeadDecrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}