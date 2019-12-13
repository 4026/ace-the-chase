using System.Collections.Generic;
using AceTheChase.Utils;

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

        public int MaxLead;
        public int MaxPlayerSpeed;
        public int MaxPursuitSpeed;
        public int MaxControl;

        public Deck<IPlayerCard> PlayerDeck;
        public List<IPlayerCard> Hand;
        public Deck<IPlayerCard> PlayerDiscard;
        public List<IPlayerCard> PlayerExhaust;

        public Deck<IPursuitCard> PursuitDeck;
        public IPursuitCard PursuitAction;

        public Deck<IRouteCard> RouteDeck;
        public List<IRouteCard> CurrentRoute;
        public Deck<IRouteCard> RouteDiscard;

        public IList<IPlayerCard> PossibleDamage;

        public bool PlayerHasWon;

        public Chase(
            int StartingLead,
            int StartingPlayerSpeed,
            int StartingPursuitSpeed,
            int StartingControl,
            int StartingMaxLead,
            int StartingMaxPlayerSpeed,
            int StartingMaxPursuitSpeed,
            int StartingMaxControl,
            Deck<IPlayerCard> StartingPlayerDeck,
            Deck<IRouteCard> StartingRouteDeck,
            Deck<IPursuitCard> StartingPursuitDeck,
            IList<IPlayerCard> PossibleDamage
        )
        {
            this.Lead = StartingLead;
            this.PlayerSpeed = StartingPlayerSpeed;
            this.PursuitSpeed = StartingPursuitSpeed;
            this.Control = StartingControl;

            this.MaxLead = StartingMaxLead;
            this.MaxPlayerSpeed = StartingMaxPlayerSpeed;
            this.MaxPursuitSpeed = StartingMaxPursuitSpeed;
            this.MaxControl = StartingMaxControl;

            this.PlayerDeck = StartingPlayerDeck;
            this.PlayerDeck.Shuffle();
            this.PursuitDeck = StartingPursuitDeck;
            this.PursuitDeck.Shuffle();
            this.RouteDeck = StartingRouteDeck;
            this.RouteDeck.Shuffle();

            this.Hand = new List<IPlayerCard>();
            this.PlayerDiscard = new Deck<IPlayerCard>(new List<IPlayerCard>());
            this.PlayerExhaust = new List<IPlayerCard>();
            
            this.CurrentRoute = new List<IRouteCard>();
            this.RouteDiscard = new Deck<IRouteCard>(new List<IRouteCard>());

            this.PossibleDamage = PossibleDamage;

            this.PlayerHasWon = false;
        }
    }
}