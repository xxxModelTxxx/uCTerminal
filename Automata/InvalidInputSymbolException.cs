using System;

namespace EMP.Automata
{
    /// <summary>
    /// Represents InvalidInputSymbolException class for finite state machine.
    /// </summary>
    /// <typeparam name="TInSymbol">Generic type representing symbol of input alphabet. </typeparam>
    /// <typeparam name="TOutSymbol">Generic type representing token that may be carried by state.</typeparam>
    [Serializable()]
    public class InvalidInputSymbolException<TInSymbol, TOutSymbol> : Exception
    {
        /// <summary>
        /// Returns instance of InvalidInputSymbolException class.
        /// </summary>
        /// <param name="currentState">Current state.</param>
        /// <param name="symbol">Current input sumbol.</param>
        public InvalidInputSymbolException(State<TInSymbol, TOutSymbol> currentState, TInSymbol symbol)
            : base()
        {
            CurrentState = currentState;
            TransitionSymbol = symbol;
        }

        /// <summary>
        /// Returns current state upon wchich exception was thrown.
        /// </summary>
        public State<TInSymbol, TOutSymbol> CurrentState { get; }
        /// <summary>
        /// Represents input transition symbol which caused exception.
        /// </summary>
        public TInSymbol TransitionSymbol { get; }
    }
}
