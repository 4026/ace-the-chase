using System;
using System.Collections.Generic;
using AceTheChase.UI;
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
        private Sprite cardImage;

        public string Name => displayName;
        public RouteCardType CardType => cardType;
        public string Description => description;
        public string FlavourText => flavourText;
        public Sprite CardImage => cardImage;
        public Guid GUID => Guid.Empty;

        public int CompareTo(object obj)
        {
            return -1;
        }

        public abstract Chase Play(Chase currentState, UIManager uiManager);
    }
}