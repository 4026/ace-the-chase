﻿using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Trade speed for control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/BeyondTheLimit", fileName = "Player_BeyondTheLimit")]
    public class BeyondTheLimit : PlayerCard
    {
        public int LeadIncrease;
        public int DamageIntoDeck;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddLead(LeadIncrease)
                .AddDamageToTopOfDeck(DamageIntoDeck)
                .DiscardFromHand(this)
                .Done();
        }
    }
}