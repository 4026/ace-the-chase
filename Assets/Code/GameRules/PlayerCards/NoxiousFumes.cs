using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Reduce pursuit speed proportional to the number of cards in the exhaust.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Noxious Fumes", fileName = "Player_NoxiousFumes")]
    public class NoxiousFumes : PlayerCard
    {
        public float PursuitSpeedDecreasePerExhaustCard;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {
            int pursuitSpeedDelta = Mathf.FloorToInt(
                -1 * PursuitSpeedDecreasePerExhaustCard * currentState.PlayerExhaust.Count
            );

            return new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddPursuitSpeed(pursuitSpeedDelta)
                .DiscardFromHand(this)
                .Done();
        }
    }
}