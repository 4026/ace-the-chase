using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.DamageCards
{
    /// <summary>
    /// Damage that causes more damage...
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Damage/Wierd Rattling Noise", fileName = "Damage_WeirdRattlingNoise")]
    public class WeirdRattlingNoise : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Damage;
        
        public int Damage;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager)
                .AddDamageToDiscardPile(1)
                .DiscardFromHand(this)
                .Done();
        }
    }
}