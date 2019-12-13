using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Discard any route card, but take damage.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Power On Through", fileName = "Player_PowerOnThrough")]
    public class PowerOnThrough : PlayerCard
    {
        public int RouteCardsDiscarded;
        public int DamageCardsGiven;

        public override PlayerCardType CardType => PlayerCardType.Diversion;

        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // Power on Through can target any route card.
            return new CardParameterProvider<IRouteCard>(
                chaseState.CurrentRoute,
                RouteCardsDiscarded
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
                .AddDamageToTopOfDeck(DamageCardsGiven);

            foreach (IRouteCard targetCard in targetCards)
            {
                if (targetCard != null)
                {
                    muta.DiscardFromRoute(targetCard);
                }
            }

            return muta
                .DiscardFromHand(this)
                .Done();
        }
    }
}