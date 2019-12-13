using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Resurrect a card from the trash into your hand.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Capacious Toolbox", fileName = "Player_CapaciousToolbox")]
    public class CapaciousToolbox : PlayerCard
    {
        public int NumCardsResurrected;
        
        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Capacious Toolbox needs to target a card in the player's exhaust.
            return new CardParameterProvider<IPlayerCard>(
                chaseState.PlayerExhaust,
                NumCardsResurrected
            );
        }

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            int cardsShuffled = currentState.PlayerDiscard.Count;

            ChaseMutator mutator = new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this);

            foreach (IPlayerCard repairedDamage in targetCards)
            {
                if (repairedDamage != null)
                {
                    mutator.ResurrectFromExhaustToHand(repairedDamage);
                }
            }

            return mutator
                .DiscardFromHand(this)
                .Done();
        }
    }
}