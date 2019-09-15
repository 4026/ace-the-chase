using System.Collections.Generic;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Perform a repair in exchange for control.
    /// </summary>
    public class RollingRepairs : PlayerCard
    {
        public override PlayerCardType CardType => PlayerCardType.Repair;
        
        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters
        )
        {
            IPlayerCard repairedDamage = additionalParameters["repairedDamage"] as IPlayerCard;

            return new ChaseMutator(currentState)
                .ExhaustFromHand(repairedDamage)
                .AddControl(-this.ControlCost)
                .DiscardFromHand(this)
                .Done();
        }
    }
}