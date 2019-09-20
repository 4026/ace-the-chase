using System.Collections.Generic;
using AceTheChase.UI;
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
            IDictionary<string, object> additionalParameters,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager)
                .AddControl(-this.ControlDecrease)
                .ActivateCard(this)
                .DiscardFromHand(this)
                .Done();
        }
    }
}