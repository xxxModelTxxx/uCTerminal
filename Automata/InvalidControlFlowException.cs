﻿using System;
using System.Collections.Generic;

namespace EMP.Automata
{
    /// <summary>
    /// Represents InvalidControlFlowException class for finite state machine.
    /// </summary>
    /// <typeparam name="TInSymbol">Generic type representing symbol of input alphabet. </typeparam>
    /// <typeparam name="TOutSymbol">Generic type representing token that may be carried by state.</typeparam>
    [Serializable()]
    public class InvalidControlFlowException<TInSymbol, TOutSymbol> : Exception
    {
        /// <summary>
        /// Returns instance of InvalidControlFlowException class.
        /// </summary>
        /// <param name="currentState">Current state.</param>
        /// <param name="symbol">Current input sumbol.</param>
        public InvalidControlFlowException(State<TInSymbol, TOutSymbol> currentState, List<TInSymbol> symbols)
            : base()
        {
            CurrentState = currentState;
            TransitionSymbol = symbols;
        }

        /// <summary>
        /// Returns current state upon wchich exception was twhrown.
        /// </summary>
        public State<TInSymbol, TOutSymbol> CurrentState { get; }
        /// <summary>
        /// Snapshot of input buffer which caused exception.
        /// </summary>
        public List<TInSymbol> TransitionSymbol { get; }
    }
}
