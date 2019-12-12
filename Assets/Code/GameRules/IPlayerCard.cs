using System;
using System.Collections.Generic;
using AceTheChase.UI;
using UnityEngine.UI;

namespace AceTheChase.GameRules
{
    /// <summary>
    /// A card that the player can add to their deck for use in a chase.
    /// </summary>
    public interface IPlayerCard : ICard
    {
        /// <summary>
        /// How much control it costs to play this card.false (may be 0).
        /// </summary>
        int ControlCost { get; }

        /// <summary>
        /// The driver personality associated with this card, if any. 
        /// </summary>
        PlayerCardDriver Driver { get; }

        /// <summary>
        /// The special type of this card, if any.
        /// </summary>
        PlayerCardType CardType { get; }

        /// <summary>
        /// Get the parameter provider for this card, if there is one (null if the card requires no
        /// parameters).
        /// </summary>
        IProvidesCardParameters GetParameterProvider(Chase chaseState);

        /// <summary>
        /// Given the current chase state and a list of target cards, mutate and return the state
        /// to represent the effect of playing this card.
        /// </summary>
        Chase Play(
            Chase currentState,
            List<ICard> targetCards,
            UIManager uiManager
        );

        /// <summary>
        /// Returns true if, in the provided chase state, the player may play this card.
        /// </summary>
        bool CanPlay(Chase currentState);

    }
}