using System;
using System.Collections.Generic;
using AceTheChase.UI;

namespace AceTheChase.GameRules
{
    /// <summary>
    /// Contract for objects that provide parameters for cards. For example, the route card that 
    /// the player wants to target with their stunt.
    /// </summary>
    public interface IProvidesCardParameters
    {
        /// <summary>
        /// Begin the process of obtaining values for the card's paramters. Accepts callback
        /// functions that will be invoked with the card parameters once done or when the user
        /// cancels the parameter dialogue, respectively.
        /// </summary>
        void PromptForParameters(
            Chase currentChaseState,
            UIManager uiManager,
            Action<List<ICard>> OnComplete,
            Action OnCancel
        );
    }
}