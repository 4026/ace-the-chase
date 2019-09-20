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
        public event Action<ICard> CardClicked;

        public event Action CardPickerCancelled;

        public Text LeadLabel;
        public Text PlayerSpeedLabel;
        public Text PursuitSpeedLabel;
        public Text ControlLabel;
        public Text PlayerDeckCountLabel;
        public Text PlayerDiscardCountLabel;
        public Text RouteDeckCountLabel;


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

        private List<KeyValuePair<Animator, string>> AnimationQueue;

        void Start()
        {
            CardSpawnParents[CardSpawnLocation.Hand] = Hand;
            CardSpawnParents[CardSpawnLocation.Route] = Route;
            CardSpawnParents[CardSpawnLocation.Pursuit] = Pursuit;
            CardSpawnParents[CardSpawnLocation.CardPicker] = CardPicker.CardGrid;
            AnimationQueue = new List<KeyValuePair<Animator, string>>();
        }

        /// <summary>
        /// Reset the entire UI to reflect the provided chase state.
        /// </summary>
        public void SetState(Chase chaseState)
        {
            this.LeadLabel.text = chaseState.Lead.ToString("N0");
            this.PlayerSpeedLabel.text = chaseState.PlayerSpeed.ToString("N0");
            this.PursuitSpeedLabel.text = chaseState.PursuitSpeed.ToString("N0");
            this.ControlLabel.text = chaseState.Control.ToString("N0");

            this.PlayerDeckCountLabel.text = chaseState.PlayerDeck.Count.ToString("N0");
            this.PlayerDiscardCountLabel.text = chaseState.PlayerDiscard.Count.ToString("N0");
            this.RouteDeckCountLabel.text = chaseState.RouteDeck.Count.ToString("N0");

            foreach (Transform child in this.Hand.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (IPlayerCard card in chaseState.Hand)
            {
                this.SpawnCard(card, CardSpawnLocation.Hand);
            }

            foreach (Transform child in this.Route.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (IPlayerCard card in chaseState.CurrentRoute)
            {
                this.SpawnCard(card, CardSpawnLocation.Route);
            }

            foreach (Transform child in this.Pursuit.transform)
            {
                Destroy(child.gameObject);
            }
            if (chaseState.PursuitAction != null)
            {
                this.SpawnCard(chaseState.PursuitAction, CardSpawnLocation.Pursuit);
            }
        }

        public void AddAnimationToQueue(Animator animator, string trigger)
        {
            KeyValuePair<Animator, string> keyValuePair = new KeyValuePair<Animator, string>(animator, trigger);
            AnimationQueue.Add(keyValuePair);
            if (AnimationQueue.Count == 1)
            {
                AnimationQueue[0].Key.SetTrigger(AnimationQueue[0].Value);
            }
        }

        public void AnimationEnded()
        {
            if (AnimationQueue.Count > 0)
            {
                AnimationQueue[0].Key.ResetTrigger(AnimationQueue[0].Value);
                AnimationQueue.RemoveAt(0);
                if (AnimationQueue.Count > 0)
                {
                    AnimationQueue[0].Key.SetTrigger(AnimationQueue[0].Value);
                }
            }
            else
            {
                Debug.LogError("No animations to end");
            }
        }

        public bool HasFinishedAnimations()
        {
            return (AnimationQueue.Count == 0);
        }

        /// <summary>
        /// Spawn a new card prefab at the specified location.
        /// </summary>
        public GameObject SpawnCard<TCard>(TCard card, CardSpawnLocation location) 
            where TCard : ICard
        {

            GameObject parent = this.CardSpawnParents[location];
            GameObject newCard = Instantiate(UICardPrefab, parent.transform);
            UICardView cardComponent = newCard.GetComponent<UICardView>();
            newCard.GetComponent<AnimationEndedTrigger>().SetUIManager(this);

            PlayerCard playerCard = card as PlayerCard;
            if (playerCard != null)
            {
                cardComponent.Setup(playerCard);
                cardComponent.OnClick += OnCardClicked;
                cardComponent.OnClick += OnPlayerCardClicked;
                return newCard;
            }
            
            RouteCard routeCard = card as RouteCard;
            if (routeCard != null)
            {
                cardComponent.Setup(routeCard);
                cardComponent.OnClick += OnCardClicked;
                cardComponent.OnClick += OnRouteCardClicked;
                return newCard;
            }

            PursuitCard pursuitCard = card as PursuitCard;
            if (pursuitCard != null)
            {
                cardComponent.Setup(pursuitCard);
                cardComponent.OnClick += OnCardClicked;
                cardComponent.OnClick += OnPursuitCardClicked;
                return newCard;
            }

            throw new ArgumentException($"Don't know how to set up a UI card object for a {card.GetType().FullName}.");
        }

        private void OnPlayerCardClicked(object sender, EventArgs e)
        {
            IPlayerCard clickedCard = (sender as GameObject)
                ?.GetComponent<UICardView>()
                ?.GetCard() as IPlayerCard;

            this.PlayerCardClicked?.Invoke(clickedCard);
        }

        private void OnCardClicked(object sender, EventArgs e)
        {
            ICard clickedCard = (sender as GameObject)
                ?.GetComponent<UICardView>()
                ?.GetCard() as ICard;

            this.CardClicked?.Invoke(clickedCard);
        }

        private void OnRouteCardClicked(object sender, EventArgs e)
        {
            IRouteCard clickedCard = (sender as GameObject)
                ?.GetComponent<UICardView>()
                ?.GetCard() as IRouteCard;

            this.RouteCardClicked?.Invoke(clickedCard);
        }

        private void OnPursuitCardClicked(object sender, EventArgs e)
        {
            IPursuitCard clickedCard = (sender as GameObject)
                ?.GetComponent<UICardView>()
                ?.GetCard() as IPursuitCard;

            this.PursuitCardClicked?.Invoke(clickedCard);
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

        public void HideCardPicker()
        {
            this.CardPicker.gameObject.SetActive(false);
        }

        public void CancelCardPicker()
        {
            this.CardPicker.gameObject.SetActive(false);
            this.CardPickerCancelled?.Invoke();
        }

        /// <summary>
        /// Queue an animation to change the player's current lead.
        /// </summary>
        public void AnimateLeadChange(int delta, Chase newState)
        {
            this.LeadLabel.text = newState.Lead.ToString("N0");
        }

        /// <summary>
        /// Queue an animation to change the player's current speed.
        /// </summary>
        public void AnimatePlayerSpeedChange(int delta, Chase newState)
        {
            this.PlayerSpeedLabel.text = newState.PlayerSpeed .ToString("N0");
        }

        /// <summary>
        /// Queue an animation to change the current pursuit speed.
        /// </summary>
        public void AnimatePursuitSpeedChange(int delta, Chase newState)
        {
            this.PursuitSpeedLabel.text = newState.PursuitSpeed .ToString("N0");
        }

        /// <summary>
        /// Queue an animation to change the player's current control.
        /// </summary>
        public void AnimateControlChange(int delta, Chase newState)
        {
            this.ControlLabel.text = newState.Control.ToString("N0");
        }

        /// <summary>
        /// Queue an animation to show the drawing of a card into the player's hand.
        /// </summary>
        public void AnimateCardDraw(IPlayerCard card, Chase newState)
        {
            GameObject newCard = this.SpawnCard(card, CardSpawnLocation.Hand);
            UICardView uiCardView = newCard.GetComponent<UICardView>();
            uiCardView.Anim.SetTrigger("PreDraw");
            AddAnimationToQueue(uiCardView.Anim, "Draw");
            this.PlayerDeckCountLabel.text = newState.PlayerDeck.Count.ToString("N0");
        }

        /// <summary>
        /// Queue an animation to show the addition of a card to the current route.
        /// </summary>
        public void AnimateRouteCardDraw(IRouteCard card, Chase newState)
        {
            this.SpawnCard(card, CardSpawnLocation.Route);
            this.RouteDeckCountLabel.text = newState.RouteDeck.Count.ToString("N0");
        }

        /// <summary>
        /// Queue an animation to show a card being discarded from the player's hand.
        /// </summary>
        public void AnimateDiscard(IPlayerCard card, Chase newState)
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
                    Destroy(child.gameObject);
                    break;
                }
            }

            this.PlayerDiscardCountLabel.text = newState.PlayerDiscard.Count.ToString("N0");
        }

        /// <summary>
        /// Queue an animation to show a card being exhausted from the player's hand.
        /// </summary>
        public void AnimateExhaust(IPlayerCard card, Chase newState)
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
                    Destroy(child.gameObject);
                    break;
                }
            }
        }

        /// <summary>
        /// Queue an animation to show a card being removed from the current route.
        /// </summary>
        public void AnimateRouteDiscard(IRouteCard card, Chase newState)
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
                    Destroy(child.gameObject);
                    break;
                }
            }
        }

        /// <summary>
        /// Queue an animation to show the swapping out of a pursuit card for a new one.
        /// </summary>
        public void AnimatePursuitChange(IPursuitCard card, Chase newState)
        {
            foreach (Transform child in this.Pursuit.transform)
            {
                Destroy(child.gameObject);
            }

            this.SpawnCard(card, CardSpawnLocation.Pursuit);
        }

        /// <summary>
        /// Queue an animation to show the player's deck being recycled.
        /// </summary>
        public void AnimatePlayerDeckRecycle(Chase newState)
        {
            this.PlayerDeckCountLabel.text = newState.PlayerDeck.Count.ToString("N0");
            this.PlayerDiscardCountLabel.text = newState.PlayerDiscard.Count.ToString("N0");
        }

        /// <summary>
        /// Queue an animation to show the route deck being recycled.
        /// </summary>
        public void AnimateRouteDeckRecycle(Chase newState)
        {
            this.RouteDeckCountLabel.text = newState.RouteDeck.Count.ToString("N0");
        }

        /// <summary>
        /// Queue an animation to show a damage card being added to the player's deck.
        /// </summary>
        public void AnimateDamageToDeck(IPlayerCard card, Chase newState)
        {
            this.PlayerDeckCountLabel.text = newState.PlayerDeck.Count.ToString("N0");
        }

        /// <summary>
        /// Queue an animation to show a damage card being added to the player's discard pile.
        /// </summary>
        public void AnimateDamageToDiscard(IPlayerCard card, Chase newState)
        {
            this.PlayerDiscardCountLabel.text = newState.PlayerDiscard.Count.ToString("N0");
        }
    }
}
