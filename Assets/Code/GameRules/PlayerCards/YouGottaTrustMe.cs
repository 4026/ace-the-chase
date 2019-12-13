using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Perform a diversion, accelerate to max speed and gain some lead, but take some damage.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/You Gotta Trust Me", fileName = "Player_YouGottaTrustMe")]
    public class YouGottaTrustMe : PlayerCard
    {
        public int LeadIncrease;
        public int DamageTaken;

        public override PlayerCardType CardType => PlayerCardType.Diversion;

        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // You Gotta Trust Me is a diversion, so it requires an Obstacle card as a parameter.
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
                .AddPlayerSpeed(currentState.MaxPlayerSpeed - currentState.PlayerSpeed)
                .AddLead(LeadIncrease)
                .AddDamageToTopOfDeck(DamageTaken);

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