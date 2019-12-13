using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Perform a diversion and gain some lead in exchange for speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Turn. NOW!", fileName = "Player_TurnNow")]
    public class TurnNow : PlayerCard
    {
        public int SpeedDecrease;
        public int LeadIncrease;

        public override PlayerCardType CardType => PlayerCardType.Diversion;

        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // "Turn. NOW!" is a diversion, so it requires an Obstacle card as a parameter.
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
                .AddPlayerSpeed(-1 * SpeedDecrease)
                .AddLead(LeadIncrease);

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