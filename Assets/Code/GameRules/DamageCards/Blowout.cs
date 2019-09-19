using System.Collections.Generic;
using UnityEngine;

namespace AceTheChase.GameRules.DamageCards
{
    /// <summary>
    /// Damage. Reduce the player's control next turn.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Damage/Blowout", fileName = "Damage_Blowout")]
    public class Blowout : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Damage;

        public int ControlDecrease;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters
        )
        {
            return new ChaseMutator(currentState)
                .AddControl(-this.ControlDecrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}