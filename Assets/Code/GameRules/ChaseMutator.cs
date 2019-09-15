using UnityEngine;

namespace AceTheChase.GameRules
{
    /// <summary>
    /// Class contianing some common methods for altering chase state.
    /// 
    /// This has been built with a "fluent" interface on the grounds that it could at some point
    /// support a factory pattern that creates a new chase state from an immutable input state.
    /// For now, though, it just mutates the input.
    /// 
    /// Alternatively (or additionally), this pattern could also accumulate a list of animations to
    /// be played in order to communicate the state changes to the player.
    /// </summary>
    public class ChaseMutator
    {
        private Chase chase;

        public ChaseMutator(Chase chaseState)
        {
            this.chase = chaseState;
        }

        /// <summary>
        /// Add the specified value to the player's current lead, keeping it within the current
        /// bounds.
        /// </summary>
        public ChaseMutator AddLead(int delta)
        {
            this.chase.Lead = Mathf.Clamp(
                this.chase.Lead + delta,
                0,
                this.chase.MaxLead
            );
            
            return this;
        }

        /// <summary>
        /// Add the specified value to the player's current speed, keeping it within the current
        /// bounds.
        /// </summary>
        public ChaseMutator AddPlayerSpeed(int delta)
        {
            this.chase.PlayerSpeed = Mathf.Clamp(
                this.chase.PlayerSpeed + delta,
                0,
                this.chase.MaxPlayerSpeed
            );

            return this;
        }

        /// <summary>
        /// Add the specified value to the current pursuit speed, keeping it within the current
        /// bounds.
        /// </summary>
        public ChaseMutator AddPursuitSpeed(int delta)
        {
            this.chase.PursuitSpeed = Mathf.Clamp(
                this.chase.PursuitSpeed + delta,
                0,
                this.chase.MaxPursuitSpeed
            );

            return this;
        }

        /// <summary>
        /// Add the specified value to the player's current control, keeping it within the current
        /// bounds.
        /// </summary>
        public ChaseMutator AddControl(int delta)
        {
            this.chase.Control = Mathf.Clamp(
                this.chase.Control + delta,
                0,
                this.chase.MaxControl
            );

            return this;
        }

        /// <summary>
        /// Draw the specified number of cards from the player's deck and add them to their hand,
        /// recycling their discard pile into their deck if necessary.
        /// </summary>
        public ChaseMutator DrawCards(int numCards)
        {
            int cardsToDrawFromThisDeck = Mathf.Min(numCards, this.chase.PlayerDeck.Count);
            int cardsToDrawAfterRecycle = numCards - cardsToDrawFromThisDeck;

            this.chase.Hand.AddRange(this.chase.PlayerDeck.Draw(cardsToDrawFromThisDeck));
            if (cardsToDrawAfterRecycle > 0)
            {
                this.RecyclePlayerDeck();
                this.chase.Hand.AddRange(
                    this.chase.PlayerDeck.Draw(
                        Mathf.Min(cardsToDrawAfterRecycle, this.chase.PlayerDeck.Count)
                    )
                );
            }

            return this;
        }

        /// <summary>
        /// Draw the specified number of route cards from the route deck and add them to the current
        /// route, recycling the rotue discard pile into the route deck if necessary.
        /// </summary>
        public ChaseMutator DrawRouteCards(int numCards)
        {
            int cardsToDrawFromThisDeck = Mathf.Min(numCards, this.chase.RouteDeck.Count);
            int cardsToDrawAfterRecycle = numCards - cardsToDrawFromThisDeck;

            this.chase.CurrentRoute.AddRange(this.chase.RouteDeck.Draw(cardsToDrawFromThisDeck));
            if (cardsToDrawAfterRecycle > 0)
            {
                this.RecycleRouteDeck();
                this.chase.CurrentRoute.AddRange(
                    this.chase.RouteDeck.Draw(
                        Mathf.Min(cardsToDrawAfterRecycle, this.chase.RouteDeck.Count)
                    )
                );
            }

            return this;
        }

        /// <summary>
        /// Discard the specified card from the player's hand.
        /// </summary>
        public ChaseMutator DiscardFromHand(IPlayerCard card)
        {
            this.chase.Hand.Remove(card);
            this.chase.PlayerDiscard.Prepend(card);

            return this;
        }

        /// <summary>
        /// Exhaust the specified card from the player's hand.
        /// </summary>
        public ChaseMutator ExhaustFromHand(IPlayerCard card)
        {
            this.chase.Hand.Remove(card);
            this.chase.PlayerTrash.Prepend(card);

            return this;
        }

        /// <summary>
        /// Discard the specified card from the current route.
        /// </summary>
        public ChaseMutator DiscardFromRoute(IRouteCard card)
        {
            this.chase.CurrentRoute.Remove(card);
            this.chase.RouteDiscard.Prepend(card);

            return this;
        }
        

        /// <summary>
        /// Replace the current pursuit action with a card randomly drawn from the pursuit deck.
        /// </summary>
        public ChaseMutator ReplacePursuitAction()
        {
            if (this.chase.PursuitAction != null)
            {
                this.chase.PursuitDeck.Append(this.chase.PursuitAction);
            }
            this.chase.PursuitDeck.Shuffle();
            this.chase.PursuitAction = this.chase.PursuitDeck.Draw(1)[0];

            return this;
        }

        /// <summary>
        /// Move all cards from the player's discard pile into their deck and shuffle it. This 
        /// should happen any time the player needs to draw cards from their deck but are unable to.
        /// </summary>
        public ChaseMutator RecyclePlayerDeck()
        {
            this.chase.PlayerDeck.Append(
                this.chase.PlayerDiscard.Draw(this.chase.PlayerDiscard.Count)
            );
            this.chase.PlayerDeck.Shuffle();

            return this;
        }

        /// <summary>
        /// Move all cards from the route discard pile into the route deck and shuffle it. This 
        /// should happen any time we need to draw more route cards but are unable to.
        /// </summary>
        public ChaseMutator RecycleRouteDeck()
        {
            this.chase.PlayerDeck.Append(
                this.chase.PlayerDiscard.Draw(this.chase.PlayerDiscard.Count)
            );
            this.chase.PlayerDeck.Shuffle();

            return this;
        }

        /// <summary>
        /// Get the mutated chase state after applying all the requested operations.
        /// </summary>
        public Chase Done()
        {
            return this.chase;
        }
    }
}