using System.Collections.Generic;
using System.Linq;
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

        public int DrawCardsPerTurn;
        public int ControlGainPerTurn;


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
                StartingPursuitDeck
            );

            BeginTurn();
        }

        /// <summary>
        /// Called at the start of the player's turn.
        /// </summary>
        void BeginTurn()
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
        void SelectCard(IPlayerCard card)
        {
            this.CurrentChaseState = card
                .Play(this.CurrentChaseState, new Dictionary<string, object>());

            if (this.CurrentChaseState.Lead <= 0)
            {
                this.PhaseManager.State = ChasePhase.Defeat;
                return;
            }
        }

        /// <summary>
        /// Called at the end of a player's turn.
        /// </summary>
        void EndTurn()
        {
            this.PhaseManager.State = ChasePhase.ResolvingPursuitAndRoute;

            // Discard any remaining cards in the player's hand.
            ChaseMutator mutator = new ChaseMutator(this.CurrentChaseState);
            
            this.CurrentChaseState.Hand.ToList()
                .ForEach(card => mutator.DiscardFromHand(card));
            
            this.CurrentChaseState = mutator.Done();

            // Apply the current pursuit card first.
            if (this.CurrentChaseState.PursuitAction != null)
            {
                this.CurrentChaseState = this.CurrentChaseState.PursuitAction
                    .Play(this.CurrentChaseState);

                if (this.CurrentChaseState.Lead <= 0)
                {
                    this.PhaseManager.State = ChasePhase.Defeat;
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
            }

            BeginTurn();
        }
    }
}