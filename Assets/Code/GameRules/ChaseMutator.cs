using System;
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

        private string reason;
        private List<string> changesApplied = new List<string>();

        public ChaseMutator(Chase chaseState, UIManager uiManager, string reason)
        {
            this.chase = chaseState;
            this.rng = new System.Random();
            this.uiManager = uiManager;
            this.reason = reason;
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

            this.changesApplied.Add($"{delta:+#;-#;0} lead");

            this.uiManager.AnimateLeadChange(delta, this.chase);
            
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

            this.changesApplied.Add($"{delta:+#;-#;0} speed");

            this.uiManager.AnimatePlayerSpeedChange(delta, this.chase);

            return this;
        }

		/// <summary>
		/// Add the specified value to the player's current speed, keeping it within the current
		/// bounds.
		/// </summary>
		public ChaseMutator AddMaxSpeed(int delta)
		{
			this.chase.MaxPlayerSpeed = this.chase.MaxPlayerSpeed + delta;
            this.changesApplied.Add($"{delta:+#;-#;0} max speed");
			this.uiManager.AnimatePlayerMaxSpeedChange(delta, this.chase);
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

            this.changesApplied.Add($"{delta:+#;-#;0} pursuit speed");

            this.uiManager.AnimatePursuitSpeedChange(delta, this.chase);

            return this;
        }

        /// <summary>
        /// Add the specified value to the player's current control, keeping it within the current
        /// bounds.
        /// </summary>
        public ChaseMutator AddControl(int delta)
        {
            // Control may be negative: this is how we apply penalties to the player's control at 
            // turn start.
            this.chase.Control = Mathf.Min(
                this.chase.Control + delta,
                this.chase.MaxControl
            );

            this.changesApplied.Add($"{delta:+#;-#;0} control");

            this.uiManager.AnimateControlChange(delta, this.chase);

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

            this.changesApplied.Add($"Drew {numCards} player cards");

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

            this.changesApplied.Add($"Drew {numCards} route cards");

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
            this.chase.Hand.Remove(card);
            this.chase.PlayerDiscard.Prepend(card);

            this.changesApplied.Add($"Discarded {card.Name} from player's hand.");

            this.uiManager.AnimateDiscard(card, this.chase);

            return this;
        }

        public ChaseMutator AddCardToRouteDeck(IRouteCard card)
        {
            this.chase.RouteDeck.Prepend(card);
            this.changesApplied.Add($"Added {card.Name} to top of route deck.");
            return this;
        }

        /// <summary>
        /// Animate the activation of a card.
        /// </summary>
        public ChaseMutator ActivateCard(ICard card)
        {
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

            this.changesApplied.Add($"Exhausted {card.Name} from player's hand.");

            this.uiManager.AnimateExhaust(card, this.chase);

            return this;
        }

        public ChaseMutator ExhaustFromRoute(IRouteCard card)
        {
            this.chase.CurrentRoute.Remove(card);

            this.changesApplied.Add($"Exhausted {card.Name} from route.");

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

            this.changesApplied.Add($"Discarded {card.Name} from route.");

            this.uiManager.AnimateRouteDiscard(card, this.chase);

            return this;
        }
        

        /// <summary>
        /// Replace the current pursuit action with a card randomly drawn from the pursuit deck.
        /// </summary>
        public ChaseMutator ReplacePursuitAction()
        {
            string oldCard = "Nothing";
            if (this.chase.PursuitAction != null)
            {
                oldCard = this.chase.PursuitAction.Name;
                this.chase.PursuitDeck.Append(this.chase.PursuitAction);
            }

            this.chase.PursuitDeck.Shuffle();
            this.chase.PursuitAction = this.chase.PursuitDeck.Draw(1)[0];

            this.changesApplied
                .Add($"Replaced {oldCard} pursuit action with {this.chase.PursuitAction.Name}.");

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
            string[] damageNames = new string[numDamage];
            for (int i = 0; i < numDamage; ++i)
            {
                IPlayerCard addedDamage = this.chase.PossibleDamage.ChooseRandom(this.rng);
                this.chase.PlayerDeck.Prepend(addedDamage);
                damageNames[i] = addedDamage.Name;
                this.uiManager.AnimateDamageToDeck(addedDamage, this.chase);
            }

            this.changesApplied.Add($"Added {numDamage} damage to top of deck: " + String.Join(", ", damageNames));

            return this;
        }

        /// <summary>
        /// Add damage cards to the player's discard pile, where they'll definitely not be a problem
        /// for ages, right?
        /// </summary>
        public ChaseMutator AddDamageToDiscardPile(int numDamage)
        {
            string[] damageNames = new string[numDamage];
            for (int i = 0; i < numDamage; ++i)
            {
                IPlayerCard addedDamage = this.chase.PossibleDamage.ChooseRandom(this.rng);
                this.chase.PlayerDiscard.Prepend(addedDamage);
                damageNames[i] = addedDamage.Name;
                this.uiManager.AnimateDamageToDiscard(addedDamage, this.chase);
            }

            this.changesApplied.Add($"Added {numDamage} damage to discard pile: " + String.Join(", ", damageNames));

            return this;
        }

        /// <summary>
        /// Set the flag indicating that the player has won.
        /// </summary>
        public ChaseMutator SetPlayerHasWon(bool value = true)
        {
            this.chase.PlayerHasWon = value;

            this.changesApplied.Add($"Set player victory flag to {value}.");

            return this;
        }

        /// <summary>
        /// Get the mutated chase state after applying all the requested operations.
        /// </summary>
        public Chase Done()
        {
            Debug.Log($"Mutated chase state as a result of {this.reason}:\n\t" + String.Join("\n\t", this.changesApplied));
            return this.chase;
        }
    }
}