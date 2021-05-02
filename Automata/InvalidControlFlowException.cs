using System;
using System.Collections.Generic;

namespace EMP.Automata
{
    /// <summary>
    /// Represents InvalidControlFlowException class for finite state machine.
    /// </summary>
    /// <typeparam name="TSymbol">Generic type representing symbol of input alphabet. </typeparam>
    /// <typeparam name="TToken">Generic type representing token that may be carried by state.</typeparam>
    [Serializable()]
    public class InvalidControlFlowException<TSymbol, TToken> : Exception
    {
        /// <summary>
        /// Returns instance of InvalidControlFlowException class.
        /// </summary>
        /// <param name="currentState">Current state.</param>
        /// <param name="symbol">Current input sumbol.</param>
        public InvalidControlFlowException(State<TSymbol, TToken> currentState, TSymbol symbol)
            : base()
        {
            CurrentState = currentState;
            TransitionSymbol = symbol;
        }

        /// <summary>
        /// Returns current state upon wchich exception was twhrown.
        /// </summary>
        public State<TSymbol, TToken> CurrentState { get; }
        /// <summary>
        /// Represents input transition symbol which caused exception.
        /// </summary>
        public TSymbol TransitionSymbol { get; }
    }
}
