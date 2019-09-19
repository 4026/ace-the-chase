using System.Collections.Generic;
using System.Linq;
using AceTheChase.UI;
using AceTheChase.Utils;
using UnityEngine;

namespace AceTheChase.GameRules
{
    public class ChaseManager : MonoBehaviour
    {
        public int StartingLead;
        public int StartingPlayerSpeed;
        public int StartingPursuitSpeed;
        public int StartingControl;

        public int StartingMaxLead;
        public int StartingMaxPlayerSpeed;
        public int StartingMaxPursuitSpeed;
        public int StartingMaxControl;       

        public Deck<IPlayerCard> StartingPlayerDeck;
        public Deck<IRouteCard> StartingRouteDeck;
        public Deck<IPursuitCard> StartingPursuitDeck;

        public List<IPlayerCard> PossibleDamage;

        public int DrawCardsPerTurn;
        public int ControlGainPerTurn;

        /// <summary>
        /// The UI Manger in the scene.
        /// </summary>
        public UIManager UiManager;

        public Chase CurrentChaseState { get; set; }
        

        public readonly StateMachine<ChasePhase> PhaseManager
            = new StateMachine<ChasePhase>(ChasePhase.Setup);

        void Start()
        {
            // Bootstrap the chase state.
            this.CurrentChaseState = new Chase(
                StartingLead,
                StartingPlayerSpeed,
                StartingPursuitSpeed,
                StartingControl,
                StartingMaxLead,
                StartingMaxPlayerSpeed,
                StartingMaxPursuitSpeed,
                StartingMaxControl,
                StartingPlayerDeck,
                StartingRouteDeck,
                StartingPursuitDeck,
                PossibleDamage
            );

            BeginTurn();
        }

        /// <summary>
        /// Called at the start of the player's turn.
        /// </summary>
        public void BeginTurn()
        {
            this.CurrentChaseState = new ChaseMutator(this.CurrentChaseState)
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
            IProvidesCardParameters parameterProvider = card.GetParameterProvider();
            if (parameterProvider == null)
            {
                // If the card doesn't require any parameters, jut play it immediately.
                this.PlayCard(card, new Dictionary<string, object>());
            } 
            else 
            {
                // Otherwise, fire up the parameter provider and play the card once parameter values
                // have been provided.
                this.PhaseManager.State = ChasePhase.SelectingTarget;
                parameterProvider.PromptForParameters(
                    this.CurrentChaseState,
                    this.UiManager,
                    cardParameters => {
                        this.PlayCard(card, cardParameters);
                        this.PhaseManager.State = ChasePhase.SelectingCard;
                    });
            }
        }

        private void PlayCard(IPlayerCard card, IDictionary<string, object> cardParameters)
        {
            this.CurrentChaseState = card.Play(this.CurrentChaseState, cardParameters);

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
            this.CurrentChaseState = new ChaseMutator(this.CurrentChaseState)
                .AddControl(-this.CurrentChaseState.Control)
                .Done();

            // Apply the current pursuit card.
            if (this.CurrentChaseState.PursuitAction != null)
            {
                this.CurrentChaseState = this.CurrentChaseState.PursuitAction
                    .Play(this.CurrentChaseState);

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
            foreach (IRouteCard routeCard in this.CurrentChaseState.CurrentRoute)
            {
                this.CurrentChaseState = routeCard.Play(this.CurrentChaseState);

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
                    .Play(this.CurrentChaseState, new Dictionary<string, object>());

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
            ChaseMutator mutator = new ChaseMutator(this.CurrentChaseState);
            
            this.CurrentChaseState.Hand.ToList()
                .ForEach(card => mutator.DiscardFromHand(card));
            
            // Finally, apply the effects of pursit speed and the player's speed
            mutator.AddPlayerSpeed(
                this.CurrentChaseState.PlayerSpeed - this.CurrentChaseState.PursuitSpeed
            );

            if (this.CurrentChaseState.Lead <= 0)
            {
                this.PhaseManager.State = ChasePhase.Defeat;
                return;
            }

            this.CurrentChaseState = mutator.Done();

            BeginTurn();
        }
    }
}