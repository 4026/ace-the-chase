using System.Collections.Generic;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Perform a diversion in exchange for control.
    /// </summary>
    public class Shortcut : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Diversion;
        
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