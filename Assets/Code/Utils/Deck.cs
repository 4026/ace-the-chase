using System.Collections.Generic;

namespace AceTheChase.Utils
{
    /// <summary>
    /// A deck (or a discard pile!) of cards.
    /// </summary>
    public class Deck<TCard>
    {
        /// <summary>
        /// The current content of the deck.
        /// </summary>
        private List<TCard> cards;

        /// <summary>
        /// The number of cards remaining in the deck.
        /// </summary>
        public int Count => this.cards.Count;

        /// <summary>
        /// Construct a new deck from a provided list of cards.
        /// </summary>
        public Deck(IList<TCard> cards)
        {
            this.cards = new List<TCard>(cards);
        }

        /// <summary>
        /// Shuffle the cards in the deck.
        /// </summary>
        public void Shuffle()
        {
            this.cards.Shuffle();
        }

        /// <summary>
        /// Draw the top n cards from the deck.
        /// </summary>
        public List<TCard> Draw(int n)
        {
            List<TCard> drawn = this.cards.GetRange(0, n);
            this.cards.RemoveRange(0, n);
            return drawn;
        }

        /// <summary>
        /// Look at the top n cards from the deck without removing them.
        /// </summary>
        public List<TCard> Peek(int n)
        {
            List<TCard> peeked = this.cards.GetRange(0, n);
            return peeked;
        }

        /// <summary>
        /// Add a card to the end of the deck.
        /// </summary>
        public void Append(TCard card)
        {
            this.cards.Add(card);
        }

        /// <summary>
        /// Add cards to the end of the deck.
        /// </summary>
        public void Append(List<TCard> cards)
        {
            this.cards.AddRange(cards);
        }

        /// <summary>
        /// Add a card to the front of the deck.
        /// </summary>
        public void Prepend(TCard card)
        {
            this.cards.Insert(0, card);
        }

        /// <summary>
        /// Add cards to the front of the deck.
        /// </summary>
        public void Prepend(List<TCard> cards)
        {
            this.cards.InsertRange(0, cards);
        }
    }
}