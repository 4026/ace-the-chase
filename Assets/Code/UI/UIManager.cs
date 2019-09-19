using System;
using System.Collections.Generic;
using AceTheChase.GameRules;
using UnityEngine;
using UnityEngine.UI;

namespace AceTheChase.UI
{
    public class UIManager : MonoBehaviour
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

        public event Action<IPlayerCard> PlayerCardClicked;
        public event Action<IRouteCard> RouteCardClicked;
        public event Action<IPursuitCard> PursuitCardClicked;

        public ChaseManager ChaseManager;

        public Text LeadLabel;
        public Text PlayerSpeedLabel;
        public Text PursuitSpeedLabel;
        public Text ControlLabel;


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
        public void DisplayCardPicker<TCard>(IList<TCard> cards) where TCard : ICard
        {
            this.CardPicker.Clear();
            foreach(ICard card in cards)
            {
                this.SpawnCard(card, CardSpawnLocation.CardPicker);
            }

            CardPicker.gameObject.SetActive(true);
        }

        /// <summary>
        /// Queue an animation to change the player's current lead.
        /// </summary>
        public void AnimateLeadChange(int delta)
        {
            this.LeadLabel.text = this.ChaseManager.CurrentChaseState.Lead.ToString("N0");
        }

        /// <summary>
        /// Queue an animation to change the player's current speed.
        /// </summary>
        public void AnimatePlayerSpeedChange(int delta)
        {
            this.PlayerSpeedLabel.text = this.ChaseManager
                .CurrentChaseState
                .PlayerSpeed
                .ToString("N0");
        }

        /// <summary>
        /// Queue an animation to change the current pursuit speed.
        /// </summary>
        public void AnimatePursuitSpeedChange(int delta)
        {
            this.PursuitSpeedLabel.text = this.ChaseManager
                .CurrentChaseState
                .PursuitSpeed
                .ToString("N0");
        }

        /// <summary>
        /// Queue an animation to change the player's current control.
        /// </summary>
        public void AnimateControlChange(int delta)
        {
            this.ControlLabel.text = this.ChaseManager.CurrentChaseState.Control.ToString("N0");
        }

        /// <summary>
        /// Queue an animation to show the drawing of a card into the player's hand.
        /// </summary>
        public void AnimateCardDraw(IPlayerCard card)
        {
            this.SpawnCard(card, CardSpawnLocation.Hand);
        }

        /// <summary>
        /// Queue an animation to show the addition of a card to the current route.
        /// </summary>
        public void AnimateRouteCardDraw(IRouteCard card)
        {
            this.SpawnCard(card, CardSpawnLocation.Route);
        }

        /// <summary>
        /// Queue an animation to show a card being discarded from the player's hand.
        /// </summary>
        public void AnimateDiscard(IPlayerCard card)
        {
            // Remove the first matching card from the player's hand.
            foreach (Transform child in this.Hand.transform)
            {
                UICardView uiCard = child.GetComponent<UICardView>();
                if (uiCard == null) 
                {
                    Debug.LogWarning("Found object in hand that didn't have a UICardView component. Disregarding...");
                    continue;
                }

                if (uiCard.GetCard() == card) 
                {
                    Destroy(child);
                    break;
                }
            }
        }

        /// <summary>
        /// Queue an animation to show a card being exhausted from the player's hand.
        /// </summary>
        public void AnimateExhaust(IPlayerCard card)
        {
            // Remove the first matching card from the player's hand.
            foreach (Transform child in this.Hand.transform)
            {
                UICardView uiCard = child.GetComponent<UICardView>();
                if (uiCard == null) 
                {
                    Debug.LogWarning("Found object in hand that didn't have a UICardView component. Disregarding...");
                    continue;
                }

                if (uiCard.GetCard() == card) 
                {
                    Destroy(child);
                    break;
                }
            }
        }

        /// <summary>
        /// Queue an animation to show a card being removed from the current route.
        /// </summary>
        public void AnimateRouteDiscard(IRouteCard card)
        {
            // Remove the first matching card from the current route.
            foreach (Transform child in this.Route.transform)
            {
                UICardView uiCard = child.GetComponent<UICardView>();
                if (uiCard == null) 
                {
                    Debug.LogWarning("Found object in route that didn't have a UICardView component. Disregarding...");
                    continue;
                }

                if (uiCard.GetCard() == card)
                {
                    Destroy(child);
                    break;
                }
            }
        }

        /// <summary>
        /// Queue an animation to show the swapping out of a pursuit card for a new one.
        /// </summary>
        public void AnimatePursuitChange(IPursuitCard card)
        {
            foreach (Transform child in this.Pursuit.transform)
            {
                Destroy(child);
            }

            this.SpawnCard(card, CardSpawnLocation.Pursuit);
        }

        /// <summary>
        /// Queue an animation to show the player's deck being recycled.
        /// </summary>
        public void AnimatePlayerDeckRecycle()
        {
            // TODO
        }

        /// <summary>
        /// Queue an animation to show the route deck being recycled.
        /// </summary>
        public void AnimateRouteDeckRecycle()
        {
            // TODO
        }

        /// <summary>
        /// Queue an animation to show a damage card being added to the player's deck.
        /// </summary>
        public void AnimateDamageToDeck(IPlayerCard card)
        {
            // TODO
        }

        /// <summary>
        /// Queue an animation to show a damage card being added to the player's discard pile.
        /// </summary>
        public void AnimateDamageToDiscard(IPlayerCard card)
        {
            // TODO
        }
    }
}