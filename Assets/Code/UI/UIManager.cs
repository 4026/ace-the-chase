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
            CardPicker, // A UI element for choosing a card from a list.
        }

        public event Action<IPlayerCard> PlayerCardSpawned;
        public event Action<IRouteCard> RouteCardSpawned;
        public event Action<IPursuitCard> PursuitCardSpawned;

        public event Action<ICard> CardPicked;


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
        /// The UI object for the card picker in the scene.
        /// </summary>
        public CardPicker CardPicker;

        /// <summary>
        /// The prefab to instantiate to display a card in the UI.
        /// </summary>
        public GameObject UICardPrefab;

        private Dictionary<CardSpawnLocation, GameObject> CardSpawnParents
            = new Dictionary<CardSpawnLocation, GameObject>();

        void Start()
        {
            CardSpawnParents[CardSpawnLocation.Hand] = Hand;
            CardSpawnParents[CardSpawnLocation.Route] = Route;
            CardSpawnParents[CardSpawnLocation.Pursuit] = Pursuit;
            CardSpawnParents[CardSpawnLocation.CardPicker] = CardPicker.CardGrid;
        }

        /// <summary>
        /// Spawn a new card prefab at the specified location.
        /// </summary>
        public GameObject SpawnCard<TCard>(TCard card, CardSpawnLocation location) 
            where TCard : ICard
        {

            GameObject parent = this.CardSpawnParents[location];
            GameObject newCard = Instantiate(UICardPrefab, parent.transform);

            PlayerCard playerCard = card as PlayerCard;
            if (playerCard != null)
            {
                newCard.GetComponent<UICardView>().Setup(playerCard);
                return newCard;
            }
            
            RouteCard routeCard = card as RouteCard;
            if (routeCard != null)
            {
                newCard.GetComponent<UICardView>().Setup(routeCard);
                return newCard;
            }

            PursuitCard pursuitCard = card as PursuitCard;
            if (pursuitCard != null)
            {
                newCard.GetComponent<UICardView>().Setup(pursuitCard);
                return newCard;
            }

            throw new ArgumentException($"Don't know how to set up a UI card object for a {card.GetType().FullName}.");
        }

        /// <summary>
        /// Activate the card picker popup and display the provided list of cards in it.
        /// </summary>
        public void DisplayCardPicker(IList<ICard> cards)
        {
            this.CardPicker.Clear();
            foreach(ICard card in cards)
            {
                this.SpawnCard(card, CardSpawnLocation.CardPicker);
            }

            CardPicker.gameObject.SetActive(true);
        }
    }
}