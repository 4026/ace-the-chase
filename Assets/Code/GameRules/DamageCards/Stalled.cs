using System.Collections.Generic;

namespace AceTheChase.GameRules.DamageCards
{
    /// <summary>
    /// Damage. Reduce the player's speed.
    /// </summary>
    public class Stalled : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Damage;
        
        public int SpeedDecrease;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters
        )
        {
            return new ChaseMutator(currentState)
                .AddPlayerSpeed(-this.SpeedDecrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}