using System.Collections.Generic;
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
        
        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters
        )
        {
            IRouteCard discardedRouteCard = additionalParameters["discardedRouteCard"] as IRouteCard;

            return new ChaseMutator(currentState)
                .DiscardFromRoute(discardedRouteCard)
                .AddControl(-this.ControlCost)
                .DiscardFromHand(this)
                .Done();
        }
    }
}