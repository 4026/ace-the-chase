using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Perform a stunt in exchange for control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Drift", fileName = "Player_Drift")]
    public class Drift : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Stunt;

        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Drift is a stunt, so it requires a Maneuver card as a parameter.
            return new CardParameterProvider<IRouteCard>(
                chaseState.CurrentRoute
                    .Where(card => card.CardType == RouteCardType.Maneuver)
                    .ToList()
            );
        }
        
        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            ChaseMutator muta = new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                   .AddControl(-this.ControlCost)
                   .ActivateCard(this)
                   .DiscardFromHand(this);

            foreach (IRouteCard targetCard in targetCards)
            {
                if (targetCard != null)
                {
                    muta.DiscardFromRoute(targetCard);
                }
            }

            return muta.Done();
        }
    }
}