using System;
using System.Collections.Generic;
using AceTheChase.GameRules;
using UnityEngine;

namespace AceTheChase.UI
{
    public class UIManger : MonoBehaviour
    {
        public enum CardSpawnLocation
        {
            Hand,
            Route,
            Pursuit,
            PopupGrid, // A UI element for choosing a card from a list.
        }


        /// <summary>
        /// The UI object for the player's hand in the scene.
        /// </summary>
        public GameObject Hand;

        /// <summary>
        /// The UI object for the current route in the scene.
        /// </summary>
        public GameObject Route;

        /// <summary>
        /// The UI object for the pursuit card location in the scene.
        /// </summary>
        public GameObject Pursuit;

        /// <summary>
        /// The UI object for the player's hand in the scene.
        /// </summary>
        public GameObject PopupGrid;

        /// <summary>
        /// Spawn a new player card prefab at the specified location
        /// </summary>
        public GameObject SpawnCard(IPlayerCard card, CardSpawnLocation location)
        {
            // TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Spawn a new route card prefab at the specified location
        /// </summary>
        public GameObject SpawnCard(IRouteCard card, CardSpawnLocation location)
        {
            // TODO
            throw new NotImplementedException();
        }

        /// <summary>
        /// Spawn a new pursuit card prefab at the specified location
        /// </summary>
        public GameObject SpawnCard(IPursuitCard card, CardSpawnLocation location)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}