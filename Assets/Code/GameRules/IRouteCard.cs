namespace AceTheChase.GameRules
{
    /// <summary>
    /// A card from the route deck that the player must face in a chase.
    /// </summary>
    public interface IRouteCard
    {
        /// <summary>
        /// This display name of the card.
        /// </summary>
        /// <value></value>
        string Name { get; }

        /// <summary>
        /// Given the current chase state, mutate and return the state to represent the effect of
        /// playing this card.
        /// </summary>
        Chase Play(Chase currentState);
    }
}