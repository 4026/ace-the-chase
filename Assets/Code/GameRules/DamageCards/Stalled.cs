using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.DamageCards
{
    /// <summary>
    /// Damage. Reduce the player's speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Damage/Stalled", fileName = "Damage_Stalled")]
    public class Stalled : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Damage;
        
        public int SpeedDecrease;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .ActivateCard(this)
                .AddPlayerSpeed(-this.SpeedDecrease)
                .DiscardFromHand(this)
                .Done();
        }
    }
}