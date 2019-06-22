namespace AceTheChase.GameRules
{
    /// <summary>
    /// Represents the current state of a chase.
    /// </summary>
    public class Chase
    {
        public int Lead;
        public int PlayerSpeed;
        public int PursuitSpeed;
        public int Control;

        public Deck<PlayerCard> PlayerDeck;
        public List<PlayerCard> Hand;
        public Deck<PlayerCard> PlayerDiscard;
        public Deck<PlayerCard> PlayerTrash;

        public Deck<RouteCard> RouteDeck;
        public List<RouteCard> CurrentRoute;
    }
}