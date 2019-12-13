using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Swap a route card for another.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Plan B", fileName = "Player_PlanB")]
    public class PlanB : PlayerCard
    {
        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Plan B targets any card in the current route.
            return new CardParameterProvider<IRouteCard>(chaseState.CurrentRoute.ToList());
        }

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            ChaseMutator muta = new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this);

            foreach (IRouteCard targetCard in targetCards)
            {
                if (targetCard != null)
                {
                    muta.DiscardFromRoute(targetCard);
                }
            }

            muta.DrawRouteCards(targetCards.Count);

            return muta
                .DiscardFromHand(this)
                .Done();
        }
    }
}