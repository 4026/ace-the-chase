using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;
using System.Linq;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Trade speed for control.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/JTurn", fileName = "Player_JTurn")]
    public class JTurn : PlayerCard
    {
        public int SpeedIncrease;
        public int LeadIncrease;

        public override PlayerCardType CardType => PlayerCardType.Stunt;

        public override IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // J-Turn is a stunt, so it requires a Maneuver card as a parameter.
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
            int speedIncrease = SpeedIncrease;
            int leadIncrease = LeadIncrease;
            if(currentState.PlayerSpeed != 0)
            {
                speedIncrease = 0;
                leadIncrease = 0;
            }
            ChaseMutator muta = new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddPlayerSpeed(speedIncrease)
                .AddLead(leadIncrease)
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