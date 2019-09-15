using System.Collections.Generic;

namespace AceTheChase.GameRules
{
    /// <summary>
    /// A card that the player can add to their deck for use in a chase.
    /// </summary>
    public interface IPlayerCard
    {
        /// <summary>
        /// The display name of the card.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// How much control it costs to play this card.false (may be 0).
        /// </summary>
        int ControlCost { get; }

        /// <summary>
        /// The driver personality associated with this card, if any. 
        /// </summary>
        PlayerCardDriver Driver { get; }

        /// <summary>
        /// The special type of this card, if any.
        /// </summary>
        PlayerCardType CardType { get; }

        /// <summary>
        /// The body text to display for the card.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The flavour text to display for the card.
        /// </summary>
        string FlavourText { get; }

        /// <summary>
        /// Given the current chase state and some additional paramteters about how this card is
        /// played, mutate and return the state to represent the effect of playing this card.
        /// </summary>
        Chase Play(Chase currentState, IDictionary<string, object> additionalParameters);

        /// <summary>
        /// Returns true if, in the provided chase state, the player may play this card.
        /// </summary>
        bool CanPlay(Chase currentState);
    }
}