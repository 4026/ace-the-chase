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
        public event Action<UICardView> CardClicked;

        public event Action CardPickerCancelled;
        public event Action CardPickerNoTarget;

        //lead
        public Text LeadLabel;
        public Text LeadLabelDelta;
        public Animator LeadAnimator;

        //PlayerSpeed
        public Text PlayerSpeedLabel;
        public Text PlayerSpeedLabelDelta;
        public Animator PlayerSpeedAnimator;

        //PlayerSpeed
        public Text ControlLabel;
        public Text ControlLabelDelta;
        public Animator ControlAnimator;

        //PursuitSpeed
        public Text PursuitSpeedLabel;
        public Text PursuitSpeedLabelDelta;
        public Animator PursuitSpeedAnimator;

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

        private List<QueuedAnimation> AnimationQueue;

        public struct QueuedAnimation
        {
            public Animator Animator;
            public string TriggerToSet;
            public Action CodeToRunAtStart;
            public Action CodeToRunAtEnd;
            public Action AnimationEndedTrigger;

            public QueuedAnimation(Animator anim, string trigger, Action customStartAction, Action customEndAction)
            {
                Animator = anim;
                TriggerToSet = trigger;
                CodeToRunAtStart = customStartAction;
                CodeToRunAtEnd = customEndAction;
                AnimationEndedTrigger = null;
            }

            public void Run()
            {
                if (Animator != null)
                {
                    CodeToRunAtStart?.Invoke();
                    Animator.SetTrigger(TriggerToSet);
                }
                else
                {
                    CodeToRunAtStart?.Invoke();
                    AnimationEndedTrigger?.Invoke();
                }
            }

            public void End()
            {
                CodeToRunAtEnd?.Invoke();
                if (Animator != null)
                {
                    Animator.ResetTrigger(TriggerToSet);
                }
            }
        }

        void Start()
        {
            CardSpawnParents[CardSpawnLocation.Hand] = Hand;
            CardSpawnParents[CardSpawnLocation.Route] = Route;
            CardSpawnParents[CardSpawnLocation.Pursuit] = Pursuit;
            CardSpawnParents[CardSpawnLocation.CardPicker] = CardPicker.CardGrid;
            AnimationQueue = new List<QueuedAnimation>();
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

        public void AddAnimationToQueue(QueuedAnimation queuedAnim)
        {
            queuedAnim.AnimationEndedTrigger = AnimationEnded;
            AnimationQueue.Add(queuedAnim);
            if (AnimationQueue.Count == 1)
            {
                AnimationQueue[0].Run();
            }
        }

        public void Update()
        {
            
        }

        public void AnimationEnded()
        {
            if (AnimationQueue.Count > 0)
            {
                AnimationQueue[0].End();
                AnimationQueue.RemoveAt(0);
                if (AnimationQueue.Count > 0)
                {
                    AnimationQueue[0].Run();
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
                if (location != CardSpawnLocation.CardPicker)
                {
                    cardComponent.OnClick += OnPlayerCardClicked;
                }
                return newCard;
            }
            
            RouteCard routeCard = card as RouteCard;
            if (routeCard != null)
            {
                cardComponent.Setup(routeCard);
                cardComponent.OnClick += OnCardClicked;
                if (location != CardSpawnLocation.CardPicker)
                {
                    cardComponent.OnClick += OnRouteCardClicked;
                }
                return newCard;
            }

            PursuitCard pursuitCard = card as PursuitCard;
            if (pursuitCard != null)
            {
                cardComponent.Setup(pursuitCard);
                cardComponent.OnClick += OnCardClicked;
                if (location != CardSpawnLocation.CardPicker)
                {
                    cardComponent.OnClick += OnPursuitCardClicked;
                }
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
            UICardView clickedCard = (sender as GameObject)
                ?.GetComponent<UICardView>();

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

        public void CardPickerNoTargetSelected()
        {
            this.CardPicker.gameObject.SetActive(false);
            this.CardPickerNoTarget?.Invoke();
        }

        /// <summary>
        /// Queue an animation to change the player's current lead.
        /// </summary>
        public void AnimateLeadChange(int delta, Chase newState)
        {
            AddAnimationToQueue(new QueuedAnimation(LeadAnimator, delta > 0 ? "StatUp" : "StatDown", () => {
                this.LeadLabel.text = newState.Lead.ToString("N0");
                this.LeadLabelDelta.text = delta.ToString("N0");
            }, null));
        }

        /// <summary>
        /// Queue an animation to change the player's current speed.
        /// </summary>
        public void AnimatePlayerSpeedChange(int delta, Chase newState)
        {
            AddAnimationToQueue(new QueuedAnimation(PlayerSpeedAnimator, delta > 0 ? "StatUp" : "StatDown", () => {
                this.PlayerSpeedLabel.text = newState.PlayerSpeed.ToString("N0");
                this.PlayerSpeedLabelDelta.text = delta.ToString("N0");
            }, null));
        }

		/// <summary>
		/// Queue an animation to change the player's max speed.
		/// </summary>
		public void AnimatePlayerMaxSpeedChange(int delta, Chase newState)
		{
			//AddAnimationToQueue(new QueuedAnimation(PlayerSpeedAnimator, delta > 0 ? "StatUp" : "StatDown", () => {
			//	this.PlayerSpeedLabel.text = newState.PlayerSpeed.ToString("N0");
			//	this.PlayerSpeedLabelDelta.text = delta.ToString("N0");
			//}, null));
		}

		/// <summary>
		/// Queue an animation to change the current pursuit speed.
		/// </summary>
		public void AnimatePursuitSpeedChange(int delta, Chase newState)
        {
            AddAnimationToQueue(new QueuedAnimation(PursuitSpeedAnimator, delta > 0 ? "StatUp" : "StatDown", () => {
                this.PursuitSpeedLabel.text = newState.PursuitSpeed.ToString("N0");
                this.PursuitSpeedLabelDelta.text = delta.ToString("N0");
            }, null));
        }

        /// <summary>
        /// Queue an animation to change the player's current control.
        /// </summary>
        public void AnimateControlChange(int delta, Chase newState)
        {

            AddAnimationToQueue(new QueuedAnimation(ControlAnimator, delta > 0 ? "StatUp" : "StatDown", () => {
                this.ControlLabel.text = newState.Control.ToString("N0");
                this.ControlLabelDelta.text = delta.ToString("N0");
            }, null));
        }

        /// <summary>
        /// Queue an animation to show the drawing of a card into the player's hand.
        /// </summary>
        public void AnimateCardDraw(IPlayerCard card, Chase newState)
        {
            GameObject newCard = this.SpawnCard(card, CardSpawnLocation.Hand);
            UICardView uiCardView = newCard.GetComponent<UICardView>();
            uiCardView.Anim.SetTrigger("PreDraw");
            AddAnimationToQueue(new QueuedAnimation(uiCardView.Anim, "Draw", null, null));
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

        private UICardView FindCard(Transform parent, ICard card)
        {
            foreach (Transform child in parent)
            {
                UICardView uiCard = child.GetComponent<UICardView>();
                if (uiCard == null)
                {
                    Debug.LogWarning("Found object in hand that didn't have a UICardView component. Disregarding...");
                    continue;
                }

                if (uiCard.CardRemoved)
                {
                    continue;
                }
                if (uiCard.GetCard() == card)
                {
                    if (uiCard.GetCard().GUID == card.GUID)
                    {
                        //queue card destroy
                        return uiCard;
                    }
                }
            }
            return null;
        }



        /// <summary>
        /// Queue an animation to show a card being discarded from the player's hand.
        /// </summary>
        public void AnimateDiscard(IPlayerCard card, Chase newState)
        {
            UICardView uiCard = FindCard(this.Hand.transform, card);
            if (uiCard != null) 
            {
                uiCard.CardRemoved = true;
                //queue card destroy
                AddAnimationToQueue(new QueuedAnimation(null, null, () => {
                    if (uiCard != null && uiCard.gameObject != null)
                    {
                        Destroy(uiCard.gameObject);
                    }
                    this.PlayerDiscardCountLabel.text = newState.PlayerDiscard.Count.ToString("N0");
                }, null));
            }
        }

        /// <summary>
        /// Queue an animation to show a card being exhausted from the player's hand.
        /// </summary>
        public void AnimateActivation(ICard card, Chase newState)
        {
            // Remove the first matching card from the player's hand.
            UICardView uiCard = FindCard(this.Hand.transform, card);
            if (uiCard != null)
            {
                AddAnimationToQueue(new QueuedAnimation(uiCard.Anim, "Activate", null, null));
                return;
            }
            uiCard = FindCard(this.Route.transform, card);
            if (uiCard != null)
            {
                AddAnimationToQueue(new QueuedAnimation(uiCard.Anim, "Activate", null, null));
                return;
            }
            uiCard = FindCard(this.Pursuit.transform, card);
            if (uiCard != null)
            {
                AddAnimationToQueue(new QueuedAnimation(uiCard.Anim, "Activate", null, null));
                return;
            }

        }

        /// <summary>
        /// Queue an animation to show a card being exhausted from the player's hand.
        /// </summary>
        public void AnimateExhaust(ICard card,  Chase newState)
        {
            // Remove the first matching card from the player's hand.
            UICardView uiCard = FindCard(this.Hand.transform, card);
            if (uiCard != null)
            {
                uiCard.CardRemoved = true;
                AddAnimationToQueue(new QueuedAnimation(null, null, () => { Destroy(uiCard.gameObject); }, null));
                return;
            }
            uiCard = FindCard(this.Route.transform, card);
            if (uiCard != null)
            {
                uiCard.CardRemoved = true;
                AddAnimationToQueue(new QueuedAnimation(null, null, () => { Destroy(uiCard.gameObject); }, null));
                return;
            }
            uiCard = FindCard(this.Pursuit.transform, card);
            if (uiCard != null)
            {
                uiCard.CardRemoved = true;
                AddAnimationToQueue(new QueuedAnimation(null, null, () => { Destroy(uiCard.gameObject); }, null));
                return;
            }
        }

        /// <summary>
        /// Queue an animation to show a card being removed from the current route.
        /// </summary>
        public void AnimateRouteDiscard(IRouteCard card, Chase newState)
        {
            // Remove the first matching card from the current route.
            UICardView uiCard = FindCard(this.Route.transform, card);
            if (uiCard != null)
            {
                //queue card destroy
                AddAnimationToQueue(new QueuedAnimation(null, null, () => {
                    if (uiCard != null && uiCard.gameObject != null)
                    {
                        Destroy(uiCard.gameObject);
                    }
                }, null));
            }
        }

        /// <summary>
        /// Queue an animation to show the swapping out of a pursuit card for a new one.
        /// </summary>
        public void AnimatePursuitChange(IPursuitCard card, Chase newState)
        {
            foreach (Transform child in this.Pursuit.transform)
            {
                AddAnimationToQueue(new QueuedAnimation(null, null, () => {
                    Destroy(child.gameObject);
                }, null));
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
