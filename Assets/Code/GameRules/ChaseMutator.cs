using System.Collections.Generic;
using AceTheChase.UI;
using AceTheChase.Utils;
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
        private System.Random rng;
        private UIManager uiManager;

        public ChaseMutator(Chase chaseState, UIManager uiManager)
        {
            this.chase = chaseState;
            this.rng = new System.Random();
            this.uiManager = uiManager;
        }

        /// <summary>
        /// Add the specified value to the player's current lead, keeping it within the current
        /// bounds.
        /// </summary>
        public ChaseMutator AddLead(int delta)
        {
            Debug.Log($"Applying {delta} lead");
            this.chase.Lead = Mathf.Clamp(
                this.chase.Lead + delta,
                0,
                this.chase.MaxLead
            );

            this.uiManager.AnimateLeadChange(delta, this.chase);
            
            return this;
        }

        /// <summary>
        /// Add the specified value to the player's current speed, keeping it within the current
        /// bounds.
        /// </summary>
        public ChaseMutator AddPlayerSpeed(int delta)
        {
            Debug.Log($"Applying {delta} speed");
            this.chase.PlayerSpeed = Mathf.Clamp(
                this.chase.PlayerSpeed + delta,
                0,
                this.chase.MaxPlayerSpeed
            );

            this.uiManager.AnimatePlayerSpeedChange(delta, this.chase);

            return this;
        }

        /// <summary>
        /// Add the specified value to the current pursuit speed, keeping it within the current
        /// bounds.
        /// </summary>
        public ChaseMutator AddPursuitSpeed(int delta)
        {
            Debug.Log($"Applying {delta} pursuit speed");
            this.chase.PursuitSpeed = Mathf.Clamp(
                this.chase.PursuitSpeed + delta,
                0,
                this.chase.MaxPursuitSpeed
            );

            this.uiManager.AnimatePursuitSpeedChange(delta, this.chase);

            return this;
        }

        /// <summary>
        /// Add the specified value to the player's current control, keeping it within the current
        /// bounds.
        /// </summary>
        public ChaseMutator AddControl(int delta)
        {
            Debug.Log($"Applying {delta} control");
            // Control may be negative: this is how we apply penalties to the player's control at 
            // turn start.
            this.chase.Control = Mathf.Min(
                this.chase.Control + delta,
                this.chase.MaxControl
            );

            this.uiManager.AnimateControlChange(delta, this.chase);

            return this;
        }

        /// <summary>
        /// Draw the specified number of cards from the player's deck and add them to their hand,
        /// recycling their discard pile into their deck if necessary.
        /// </summary>
        public ChaseMutator DrawCards(int numCards)
        {
            Debug.Log($"Applying {numCards} cardDraw");
            int cardsToDrawFromThisDeck = Mathf.Min(numCards, this.chase.PlayerDeck.Count);
            int cardsToDrawAfterRecycle = numCards - cardsToDrawFromThisDeck;

            List<IPlayerCard> drawnCards = this.chase.PlayerDeck.Draw(cardsToDrawFromThisDeck);
            if (cardsToDrawAfterRecycle > 0)
            {
                this.RecyclePlayerDeck();
                drawnCards.AddRange(
                    this.chase.PlayerDeck.Draw(
                        Mathf.Min(cardsToDrawAfterRecycle, this.chase.PlayerDeck.Count)
                    )
                );
            }

            this.chase.Hand.AddRange(drawnCards);
            foreach (IPlayerCard card in drawnCards)
            {
                this.uiManager.AnimateCardDraw(card, this.chase);
            }

            return this;
        }

        /// <summary>
        /// Draw the specified number of route cards from the route deck and add them to the current
        /// route, recycling the rotue discard pile into the route deck if necessary.
        /// </summary>
        public ChaseMutator DrawRouteCards(int numCards)
        {
            Debug.Log($"Applying {numCards} routes");
            int cardsToDrawFromThisDeck = Mathf.Min(numCards, this.chase.RouteDeck.Count);
            int cardsToDrawAfterRecycle = numCards - cardsToDrawFromThisDeck;

            List<IRouteCard> drawnCards = this.chase.RouteDeck.Draw(cardsToDrawFromThisDeck);
            
            if (cardsToDrawAfterRecycle > 0)
            {
                this.RecycleRouteDeck();
                drawnCards.AddRange(
                    this.chase.RouteDeck.Draw(
                        Mathf.Min(cardsToDrawAfterRecycle, this.chase.RouteDeck.Count)
                    )
                );
            }

            this.chase.CurrentRoute.AddRange(drawnCards);
            foreach (IRouteCard card in drawnCards)
            {
                this.uiManager.AnimateRouteCardDraw(card, this.chase);
            }

            return this;
        }

        /// <summary>
        /// Discard the specified card from the player's hand.
        /// </summary>
        public ChaseMutator DiscardFromHand(IPlayerCard card)
        {
            Debug.Log($"Applying {card} discard");
            this.chase.Hand.Remove(card);
            this.chase.PlayerDiscard.Prepend(card);

            this.uiManager.AnimateDiscard(card, this.chase);

            return this;
        }

        /// <summary>
        /// Exhaust the specified card from the player's hand.
        /// </summary>
        public ChaseMutator ActivateCard(IPlayerCard card)
        {
            this.chase.Hand.Remove(card);
            this.chase.PlayerTrash.Prepend(card);

            this.uiManager.AnimateActivation(card, this.chase);

            return this;
        }

        /// <summary>
        /// Exhaust the specified card from the player's hand.
        /// </summary>
        public ChaseMutator ExhaustFromHand(IPlayerCard card)
        {
            this.chase.Hand.Remove(card);
            this.chase.PlayerTrash.Prepend(card);

            this.uiManager.AnimateExhaust(card, this.chase);

            return this;
        }

        /// <summary>
        /// Discard the specified card from the current route.
        /// </summary>
        public ChaseMutator DiscardFromRoute(IRouteCard card)
        {
            this.chase.CurrentRoute.Remove(card);
            this.chase.RouteDiscard.Prepend(card);

            this.uiManager.AnimateRouteDiscard(card, this.chase);

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

            this.uiManager.AnimatePursuitChange(this.chase.PursuitAction, this.chase);

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

            this.uiManager.AnimatePlayerDeckRecycle(this.chase);

            return this;
        }

        /// <summary>
        /// Move all cards from the route discard pile into the route deck and shuffle it. This 
        /// should happen any time we need to draw more route cards but are unable to.
        /// </summary>
        public ChaseMutator RecycleRouteDeck()
        {
            this.chase.RouteDeck.Append(
                this.chase.RouteDiscard.Draw(this.chase.RouteDiscard.Count)
            );
            this.chase.RouteDeck.Shuffle();

            this.uiManager.AnimateRouteDeckRecycle(this.chase);

            return this;
        }

        /// <summary>
        /// Add damage cards to the top of the player's deck, where they'll be drawn next turn.
        /// </summary>
        public ChaseMutator AddDamageToTopOfDeck(int numDamage)
        {
            for (int i = 0; i < numDamage; ++i)
            {
                IPlayerCard addedDamage = this.chase.PossibleDamage.ChooseRandom(this.rng);
                this.chase.PlayerDeck.Prepend(addedDamage);
                this.uiManager.AnimateDamageToDeck(addedDamage, this.chase);
            }

            return this;
        }

        /// <summary>
        /// Add damage cards to the player's discard pile, where they'll definitely not be a problem
        /// for ages, right?
        /// </summary>
        public ChaseMutator AddDamageToDiscardPile(int numDamage)
        {
            for (int i = 0; i < numDamage; ++i)
            {
                IPlayerCard addedDamage = this.chase.PossibleDamage.ChooseRandom(this.rng);
                this.chase.PlayerDiscard.Prepend(addedDamage);
                this.uiManager.AnimateDamageToDiscard(addedDamage, this.chase);
            }

            return this;
        }

        /// <summary>
        /// Set the flag indicating that the player has won.
        /// </summary>
        public ChaseMutator SetPlayerHasWon(bool value = true)
        {
            this.chase.PlayerHasWon = value;

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