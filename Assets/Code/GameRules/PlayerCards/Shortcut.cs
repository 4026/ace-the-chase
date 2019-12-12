using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Perform a diversion in exchange for control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Shortcut", fileName = "Player_Shortcut")]
    public class Shortcut : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Diversion;

        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Shortcut is a diversion, so it requires an Obstacle card as a parameter.
            return new CardParameterProvider<IRouteCard>(
                chaseState.CurrentRoute
                    .Where(card => card.CardType == RouteCardType.Obstacle)
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