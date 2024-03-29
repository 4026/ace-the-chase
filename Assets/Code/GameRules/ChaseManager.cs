using System;
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
            
            this.UiManager.PlayerCardClicked += (IPlayerCard card) => {
                if (this.PhaseManager.State == ChasePhase.SelectingCard) {
                    SelectCard(card);
                }
            };

            BeginTurn();
        }

        /// <summary>
        /// Called at the start of the player's turn.
        /// </summary>
        public void BeginTurn()
        {
            this.CurrentChaseState = new ChaseMutator(this.CurrentChaseState, this.UiManager, "starting new turn")
                .DrawCards(DrawCardsPerTurn)
                .ReplacePursuitAction()
                .DrawRouteCards(this.CurrentChaseState.PlayerSpeed)
                .AddControl(ControlGainPerTurn)
                .Done();
            
            this.PhaseManager.State = ChasePhase.SelectingCard;
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
                this.PlayCard(card, new List<ICard>());
                this.PhaseManager.State = ChasePhase.PlayingAnimation;
            } 
            else 
            {
                // Otherwise, fire up the parameter provider and play the card once parameter values
                // have been provided.
                this.PhaseManager.State = ChasePhase.SelectingParameters;

                parameterProvider.PromptForParameters(
                    this.CurrentChaseState,
                    this.UiManager,
                    targetCards => {
                        // Paramters provided, play the card.
                        this.PlayCard(card, targetCards);
                    },
                    () => {
                        // User cancelled out of parameter dialogue, go back to selecting card to
                        // play.
                        this.PhaseManager.State = ChasePhase.SelectingCard;
                    });
            }
        }

        private void PlayCard(IPlayerCard card, List<ICard> targetCards)
        {
            if (!card.CanPlay(this.CurrentChaseState))
            {
                return;
            }

            this.PhaseManager.State = ChasePhase.PlayingAnimation;
            this.CurrentChaseState = card.Play(this.CurrentChaseState, targetCards, this.UiManager);

            // Check if the player has won or lost as a result of playing this card.
            if (this.CheckForChaseEnd())
            {
                return;
            }

            this.UiManager.OnceAnimationQueueCompletes(() => { 
                this.PhaseManager.State = ChasePhase.SelectingCard; 
            });
        }

        /// <summary>
        /// Check if the chase has ended, either because the player has won or they have been 
        /// caught.
        /// </summary>
        private bool CheckForChaseEnd()
        {
            if (this.CurrentChaseState.Lead <= 0)
            {
                this.PhaseManager.State = ChasePhase.Defeat;
                this.UiManager.OnceAnimationQueueCompletes(() => { 
                    UnityEngine.SceneManagement.SceneManager.LoadScene("ResultsLose");
                });
                return true;
            }
            else if (this.CurrentChaseState.PlayerHasWon)
            {
                this.PhaseManager.State = ChasePhase.Victory;
                this.UiManager.OnceAnimationQueueCompletes(() => { 
                    UnityEngine.SceneManagement.SceneManager.LoadScene("ResultsWin");
                });
                return true;
            }

            return false;
        }

        /// <summary>
        /// Called at the end of a player's turn.
        /// </summary>
        public void EndTurn()
        {
            if(!UiManager.HasFinishedAnimations())
            {
                return;
            }

            this.PhaseManager.State = ChasePhase.ResolvingPursuitAndRoute;

            // The player discards all remaining cards and loses any unspent control.
            ChaseMutator endPlayerTurnMutator = new ChaseMutator(
                this.CurrentChaseState,
                this.UiManager,
                "ending player turn"
            );
            endPlayerTurnMutator.AddControl(-this.CurrentChaseState.Control);
            this.CurrentChaseState.Hand.ToList()
                .ForEach(card => endPlayerTurnMutator.DiscardFromHand(card));
            this.CurrentChaseState = endPlayerTurnMutator.Done();

            // Apply the current pursuit card.
            if (this.CurrentChaseState.PursuitAction != null)
            {
                this.CurrentChaseState = this.CurrentChaseState.PursuitAction
                    .Play(this.CurrentChaseState, this.UiManager);

                if (this.CheckForChaseEnd())
                {
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

                if (this.CheckForChaseEnd())
                {
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
                    .Play(this.CurrentChaseState, new List<ICard>(), this.UiManager);

                if (this.CheckForChaseEnd())
                {
                    return;
                }
            }

            // Discard any remaining cards in the player's hand.
            ChaseMutator endRoundMutator = new ChaseMutator(this.CurrentChaseState, this.UiManager, "ending round");
            
            // Finally, apply the effects of pursit speed and the player's speed
            this.CurrentChaseState = endRoundMutator
                .AddLead(this.CurrentChaseState.PlayerSpeed - this.CurrentChaseState.PursuitSpeed)
                .Done();

            if (this.CheckForChaseEnd())
            {
                return;
            }

            // Add a callback to start the next turn once the animation for the end of the turn is
            // done.
            this.UiManager.OnceAnimationQueueCompletes(() => { BeginTurn(); });
        }
    }
}
