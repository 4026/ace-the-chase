using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AceTheChase.GameRules
{
    /// <summary>
    /// The base class for all pursuit cards.
    /// </summary>
    public abstract class PursuitCard : ScriptableObject, IPursuitCard
    {
        /// <summary>
        /// The display name of the card.
        /// </summary>
        [SerializeField]
        private string displayName;

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
        public string Description => Description;
        public string FlavourText => flavourText;
        public Image CardImage => cardImage;

        public abstract Chase Play(Chase currentState);
    }
}