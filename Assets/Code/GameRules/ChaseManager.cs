using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using AceTheChase.Utils;
using UnityEngine;

namespace AceTheChase.GameRules
{
    public class ChaseManager : MonoBehaviour
    {
        public ChaseStartInfo StartInfo;

        public int DrawCardsPerTurn;
        public int ControlGainPerTurn;

        /// <summary>
        /// The UI Manger in the scene.
        /// </summary>
        public UIManager UiManager;

        public Chase CurrentChaseState { get; set; }
        

        public readonly StateMachine<ChasePhase> PhaseManager
            = new StateMachine<ChasePhase>(ChasePhase.Setup);

        void Awake()
        {
            // Bootstrap the chase state.
            this.CurrentChaseState = new Chase(
                StartInfo.StartingLead,
                StartInfo.StartingPlayerSpeed,
                StartInfo.StartingPursuitSpeed,
                StartInfo.StartingControl,
                StartInfo.StartingMaxLead,
                StartInfo.StartingMaxPlayerSpeed,
                StartInfo.StartingMaxPursuitSpeed,
                StartInfo.StartingMaxControl,
                StartInfo.StartingPlayerDeck,
                StartInfo.StartingRouteDeck,
                StartInfo.StartingPursuitDeck,
                StartInfo.AllDamageCards
            );
        }

        void Start()
        {
            this.UiManager.SetState(this.CurrentChaseState);
            BeginTurn();
        }

        void Update()
        {
            if (this.PhaseManager.State == ChasePhase.PlayingAnimation
                && this.UiManager.HasFinishedAnimations())
            {
                this.PhaseManager.State = this.PhaseManager.QueuedState;
            }
        }

        /// <summary>
        /// Called at the start of the player's turn.
        /// </summary>
        public void BeginTurn()
        {
            this.CurrentChaseState = new ChaseMutator(this.CurrentChaseState, this.UiManager)
                .DrawCards(DrawCardsPerTurn)
                .ReplacePursuitAction()
                .DrawRouteCards(this.CurrentChaseState.PlayerSpeed)
                .AddControl(ControlGainPerTurn)
                .Done();
            this.PhaseManager.QueuedState = ChasePhase.SelectingCard;
            this.PhaseManager.State = ChasePhase.PlayingAnimation;
            this.UiManager.PlayerCardClicked += SelectCard;
        }

        /// <summary>
        /// Called when the player has selected a card to play.
        /// </summary>
        public void SelectCard(IPlayerCard card)
        {
            if (!card.CanPlay(this.CurrentChaseState))
            {
                return;
            }

            IProvidesCardParameters parameterProvider = card
                .GetParameterProvider(this.CurrentChaseState);

            if (parameterProvider == null)
            {
                // If the card doesn't require any parameters, jut play it immediately.
                this.PlayCard(card, new Dictionary<string, object>());
            } 
            else 
            {
                // Otherwise, fire up the parameter provider and play the card once parameter values
                // have been provided.
                this.PhaseManager.State = ChasePhase.SelectingParameters;
                this.UiManager.PlayerCardClicked -= SelectCard;

                parameterProvider.PromptForParameters(
                    this.CurrentChaseState,
                    this.UiManager,
                    cardParameters => {
                        // Paramters provided, play the card.
                        this.PlayCard(card, cardParameters);
                        this.PhaseManager.State = ChasePhase.SelectingCard;
                        this.UiManager.PlayerCardClicked += SelectCard;
                    },
                    () => {
                        // User cancelled out of parameter dialogue, go back to selecting card to
                        // play.
                        this.PhaseManager.State = ChasePhase.SelectingCard;
                        this.UiManager.PlayerCardClicked += SelectCard;
                    });
            }
        }

        private void PlayCard(IPlayerCard card, IDictionary<string, object> cardParameters)
        {
            if (!card.CanPlay(this.CurrentChaseState))
            {
                return;
            }

            this.CurrentChaseState = card.Play(this.CurrentChaseState, cardParameters, this.UiManager);

            // Check if the player has won or lost as a result of playing this card.
            if (this.CurrentChaseState.Lead <= 0)
            {
                this.PhaseManager.State = ChasePhase.Defeat;
                return;
            }
            else if (this.CurrentChaseState.PlayerHasWon)
            {
                this.PhaseManager.State = ChasePhase.Victory;
                return;
            }
        }

        /// <summary>
        /// Called at the end of a player's turn.
        /// </summary>
        public void EndTurn()
        {
            this.PhaseManager.State = ChasePhase.ResolvingPursuitAndRoute;

            // The player loses any unspent control
            this.CurrentChaseState = new ChaseMutator(this.CurrentChaseState, this.UiManager)
                .AddControl(-this.CurrentChaseState.Control)
                .Done();

            // Apply the current pursuit card.
            if (this.CurrentChaseState.PursuitAction != null)
            {
                this.CurrentChaseState = this.CurrentChaseState.PursuitAction
                    .Play(this.CurrentChaseState, this.UiManager);

                if (this.CurrentChaseState.Lead <= 0)
                {
                    this.PhaseManager.State = ChasePhase.Defeat;
                    return;
                }
                else if (this.CurrentChaseState.PlayerHasWon)
                {
                    this.PhaseManager.State = ChasePhase.Victory;
                    return;
                }
            }


            // Then apply any route cards.
            while (this.CurrentChaseState.CurrentRoute.Count > 0)
            {
                // Play the next route card. We charitably assume that route cards discard
                // themselves after being played.
                IRouteCard routeCard = this.CurrentChaseState.CurrentRoute[0];
                this.CurrentChaseState = routeCard.Play(this.CurrentChaseState, this.UiManager);

                if (this.CurrentChaseState.Lead <= 0)
                {
                    this.PhaseManager.State = ChasePhase.Defeat;
                    return;
                }
                else if (this.CurrentChaseState.PlayerHasWon)
                {
                    this.PhaseManager.State = ChasePhase.Victory;
                    return;
                }
            }

            // Apply any damage cards in the player's hand
            IPlayerCard[] damageCards = this.CurrentChaseState.Hand
                .Where(card => card.CardType == PlayerCardType.Damage)
                .ToArray();

            foreach (IPlayerCard damageCard in damageCards)
            {

                this.CurrentChaseState = damageCard
                    .Play(this.CurrentChaseState, new Dictionary<string, object>(), this.UiManager);

                if (this.CurrentChaseState.Lead <= 0)
                {
                    this.PhaseManager.State = ChasePhase.Defeat;
                    return;
                }
                else if (this.CurrentChaseState.PlayerHasWon)
                {
                    this.PhaseManager.State = ChasePhase.Victory;
                    return;
                }
            }

            // Discard any remaining cards in the player's hand.
            ChaseMutator mutator = new ChaseMutator(this.CurrentChaseState, this.UiManager);
            
            this.CurrentChaseState.Hand.ToList()
                .ForEach(card => mutator.DiscardFromHand(card));
            
            // Finally, apply the effects of pursit speed and the player's speed
            this.CurrentChaseState = mutator.AddLead(
                this.CurrentChaseState.PlayerSpeed - this.CurrentChaseState.PursuitSpeed
            ).Done();

            if (this.CurrentChaseState.Lead <= 0)
            {
                this.PhaseManager.State = ChasePhase.Defeat;
                return;
            }

            BeginTurn();
        }
    }
}
