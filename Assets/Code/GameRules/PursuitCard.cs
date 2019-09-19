using System.Collections.Generic;
using AceTheChase.UI;
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
        private Sprite cardImage;

        public string Name => displayName;
        public string Description => description;
        public string FlavourText => flavourText;
        public Sprite CardImage => cardImage;

        public abstract Chase Play(Chase currentState, UIManager uiManager);
    }
}