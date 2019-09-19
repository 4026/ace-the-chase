namespace AceTheChase.GameRules
{
    /// <summary>
    /// A card from the route deck that the player must face in a chase.
    /// </summary>
    public interface IRouteCard : ICard
    {
        /// <summary>
        /// The special type of this card, if any.
        /// </summary>
        RouteCardType CardType { get; }

        /// <summary>
        /// Given the current chase state, mutate and return the state to represent the effect of
        /// playing this card.
        /// </summary>
        Chase Play(Chase currentState);
    }
}