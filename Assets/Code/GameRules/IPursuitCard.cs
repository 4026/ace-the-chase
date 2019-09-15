namespace AceTheChase.GameRules
{
    /// <summary>
    /// A card from the pursuit deck that the player must face in a chase.
    /// </summary>
    public interface IPursuitCard
    {
        /// <summary>
        /// The display name of the card.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The body text to display for the card.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The flavour text to display for the card.
        /// </summary>
        string FlavourText { get; }

        /// <summary>
        /// Given the current chase state, mutate and return the state to represent the effect of
        /// playing this card.
        /// </summary>
        Chase Play(Chase currentState);
    }
}