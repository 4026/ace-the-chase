using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AceTheChase.GameRules
{
    /// <summary>
    /// The base class for all route cards.
    /// </summary>
    public abstract class RouteCard : ScriptableObject, IRouteCard
    {
        /// <summary>
        /// The display name of the card.
        /// </summary>
        [SerializeField]
        private string displayName;

        /// <summary>
        /// The special type of this card, if any.
        /// </summary>
        [SerializeField]
        private RouteCardType cardType;

        /// <summary>
        /// The body text to display for the card.
        /// </summary>
        [SerializeField]
        private string description;

        /// <summary>
        /// The flavour text to display for the card.
        /// </summary>
        [SerializeField]
        private string flavourText;

        /// <summary>
        /// The card image
        /// </summary>
        [SerializeField]
        private Image cardImage;

        public string Name => displayName;
        public RouteCardType CardType => cardType;
        public string Description => Description;
        public string FlavourText => flavourText;
        public Image CardImage => cardImage;

        public abstract Chase Play(Chase currentState);
    }
}