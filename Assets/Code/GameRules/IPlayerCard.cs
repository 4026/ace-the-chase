using System.Collections.Generic;

namespace AceTheChase.GameRules
{
    /// <summary>
    /// A card that the player can add to their deck for use in a chase.
    /// </summary>
    public interface IPlayerCard
    {
        /// <summary>
        /// This display name of the card.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Given the current chase state and some additional paramteters about how this card is
        /// played, mutate and return the state to represent the effect of playing this card.
        /// </summary>
        Chase Play(Chase currentState, IDictionary<string, object> additionalParameters);
    }
}