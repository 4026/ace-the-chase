using System.Collections.Generic;

namespace AceTheChase.GameRules.PlayerCards
{
    /// <summary>
    /// Gain control, provided you have control to spare.
    /// </summary>
    public class WellOiledGearbox : PlayerCard
    {
        public int ControlIncrease;

        public override Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters
        )
        {
            return new ChaseMutator(currentState)
                .AddControl(ControlIncrease - this.ControlCost)
                .DiscardFromHand(this)
                .Done();
        }
    }
}