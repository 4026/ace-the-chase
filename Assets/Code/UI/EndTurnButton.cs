using System;
using AceTheChase.GameRules;
using UnityEngine;
using UnityEngine.UI;

namespace AceTheChase.UI
{
    /// <summary>
    /// MonoBehaviour for a button to end the player's current turn.
    /// </summary>
    public class EndTurnButton : MonoBehaviour
    {
        /// <summary>
        /// The chase manager in the scene.
        /// </summary>
        public ChaseManager ChaseManager;

        /// <summary>
        /// The button component that this script controls.
        /// </summary>
        public Button Button;

        void Start()
        {
            this.ChaseManager.PhaseManager.NotifyStateChange += OnPhaseChange;
        }

        void OnDestroy()
        {
            this.ChaseManager.PhaseManager.NotifyStateChange -= OnPhaseChange;
        }

        private void OnPhaseChange(ChasePhase oldPhase, ChasePhase newPhase)
        {
            // This button should only be interactable while the player is choosing cards to play 
            // on their turn.
            this.Button.interactable = (newPhase == ChasePhase.SelectingCard);
        }

        public void OnClick()
        {
            this.ChaseManager.EndTurn();
        }
    }
}