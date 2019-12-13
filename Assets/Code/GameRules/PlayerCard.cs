using System;
using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine;

namespace AceTheChase.GameRules
{
    /// <summary>
    /// The base class for all player cards.
    /// </summary>
    public abstract class PlayerCard : ScriptableObject, IPlayerCard, IComparable<PlayerCard>
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
        /// The driver personality associated with this card, if any. 
        /// </summary>
        [SerializeField]
        private PlayerCardType playerCardType;

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

        private Guid m_guid;

        public string Name => displayName;
        public int ControlCost => controlCost;
        public PlayerCardDriver Driver => driver;
        public virtual PlayerCardType CardType => playerCardType;
        public string Description => description;
        public string FlavourText => flavourText;
        public Sprite CardImage => cardImage;
        public Guid GUID => m_guid;

        public abstract Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        );

        void Awake()
        {
            m_guid = Guid.NewGuid();
        }

        public bool CanPlay(Chase currentState)
        {
            return currentState.Control >= this.ControlCost;
        }


        public virtual IProvidesCardParameters GetParameterProvider(Chase chaseState)
        {
            // By default, cards do not require parameters.
            return null;
        }

        public int CompareTo(PlayerCard obj)
        {
            int ret = this.Driver.CompareTo(obj.Driver);
            if (ret == 0)
            {
                return string.Compare(this.Name, obj.Name, StringComparison.CurrentCulture);
            }
            if (ret == 0)
            {
                return m_guid.CompareTo(obj.m_guid);
            }
            return ret;
        }

        public int CompareTo(object obj)
        {
            if (obj as PlayerCard)
            {
                return CompareTo((PlayerCard)obj);
            }
            return -1;
        }
    }
}
