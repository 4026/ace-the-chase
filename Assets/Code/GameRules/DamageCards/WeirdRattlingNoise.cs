
using System.Collections.Generic;

namespace AceTheChase.GameRules.DamageCards
{
    /// <summary>
    /// Damage that causes more damage...
    /// </summary>
    public class WeirdRattlingNoise : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Damage;
        
        public int Damage;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters
        )
        {
            return new ChaseMutator(currentState)
                .AddDamageToDiscardPile(1)
                .DiscardFromHand(this)
                .Done();
        }
    }
}