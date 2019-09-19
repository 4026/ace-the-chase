using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AceTheChase.GameRules
{
    /// <summary>
    /// The interface provided by all card objects in the game, regardless of type.
    /// </summary>
    public interface ICard
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
        /// The image for this card.
        /// </summary>
        Sprite CardImage { get; }
    }
}