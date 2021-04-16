using System;
using System.Collections.Generic;

namespace EMP.Automata
{
    /// <summary>
    /// Represents InvalidControlFlowException class for finite state machine.
    /// </summary>
    /// <typeparam name="TAlphabet">Generic type representing symbol of input alphabet. </typeparam>
    /// <typeparam name="TToken">Generic type representing token that may be carried by state.</typeparam>
    [Serializable()]
    public class InvalidControlFlowException<TAlphabet, TToken> : Exception where TAlphabet : IEqualityComparer<TAlphabet>
    {
        /// <summary>
        /// Returns instance of InvalidControlFlowException class.
        /// </summary>
        /// <param name="currentState">Current state.</param>
        /// <param name="symbol">Current input sumbol.</param>
        public InvalidControlFlowException(State<TAlphabet, TToken> currentState, TAlphabet symbol)
            : base()
        {
            CurrentState = currentState;
            TransitionSymbol = symbol;
        }

        /// <summary>
        /// Returns current state upon wchich exception was twhrown.
        /// </summary>
        public State<TAlphabet, TToken> CurrentState { get; }
        /// <summary>
        /// Represents input transition symbol which caused exception.
        /// </summary>
        public TAlphabet TransitionSymbol { get; }
    }
}
