using System;
using System.Collections.Generic;
using UnityEngine;

namespace AceTheChase.GameRules
{
    /// <summary>
    /// Contract for objects that provide parameters for cards. For example, the route card that 
    /// the player wants to target with their stunt.
    /// </summary>
    public interface IProvidesCardParameters
    {
        /// <summary>
        /// Begin the process of obtaining values for the card's paramters. Accepts a callback
        /// function that will be invoked with the card parameters once done.
        /// </summary>
        void PromptForParameters(Action<IDictionary<string, object>> OnComplete);
    }
}