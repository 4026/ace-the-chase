using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AceTheChase.Utils
{
    /// <summary>
    /// State machine that tracks the current state of the game and notifies subscribers when it 
    /// changes.
    /// </summary>
    public class StateMachine<TState> where TState : System.Enum
    {
        public TState QueuedState;
        /// <summary>
        /// The current state that the game is in.
        /// </summary>
        public TState State {
            get => m_state;
            set
            {
                // Notify subscribers of the state change.
                try
                {
                    NotifyStateChange?.Invoke(this.State, value);
                }
                catch(Exception e)
                {
                    Debug.LogException(e);
                }

                //Set the new state
                m_state = value;
            }
        }
        private TState m_state;

        /// <summary>
        /// An event that notifies subscribers of a change in game state. It receives two states
        /// as parameters: the old state and the new state, respectively.
        /// </summary>
        public event Action<TState, TState> NotifyStateChange;

        public StateMachine(TState startState)
        {
            m_state = startState;
        }
    }
}