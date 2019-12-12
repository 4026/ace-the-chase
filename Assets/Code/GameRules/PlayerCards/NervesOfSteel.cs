using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;
using System.Linq;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Gain control based on your current speed.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Player/Nerves Of Steel", fileName = "Player_NervesOfSteel")]
    public class NervesOfSteel : PlayerCard
    {
        public float ControlPerSpeed;

        public override Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        )
        {

            ChaseMutator muta = new ChaseMutator(currentState, uiManager, $"playing {this.Name}")
                .AddControl(-this.ControlCost)
                .ActivateCard(this)
                .AddControl(
                    Mathf.FloorToInt(ControlPerSpeed * currentState.PlayerSpeed)
                )
                .DiscardFromHand(this);

            return muta.Done();
        }
    }
}