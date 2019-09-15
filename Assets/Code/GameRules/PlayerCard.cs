using System.Collections.Generic;
using UnityEngine;

namespace AceTheChase.GameRules
{
    /// <summary>
    /// The base class for all player cards.
    /// </summary>
    public abstract class PlayerCard : ScriptableObject, IPlayerCard
    {
        /// <summary>
        /// The display name of the card.
        /// </summary>
        [SerializeField]
        private string displayName;

        /// <summary>
        /// How much control it costs to play this card.false (may be 0).
        /// </summary>
        [SerializeField]
        private int controlCost;

        /// <summary>
        /// The driver personality associated with this card, if any. 
        /// </summary>
        [SerializeField]
        private PlayerCardDriver driver;

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

        public string Name => displayName;
        public int ControlCost => controlCost;
        public PlayerCardDriver Driver => driver;
        public virtual PlayerCardType CardType => PlayerCardType.None;
        public string Description => Description;
        public string FlavourText => flavourText;

        public abstract Chase Play(
            Chase currentState,
            IDictionary<string, object> additionalParameters
        );

        public bool CanPlay(Chase currentState)
        {
            return currentState.Control >= this.ControlCost;
        }
    }
}