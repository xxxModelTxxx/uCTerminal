using System;
using System.Collections.Generic;

namespace EMP.Automata
{
    [Serializable()]
    public class InvalidControlFlowException<TAlphabet, TToken> : Exception where TAlphabet : IEqualityComparer<TAlphabet>
    {
        public InvalidControlFlowException(State<TAlphabet, TToken> currentState, TAlphabet symbol)
            : base()
        {
            CurrentState = currentState;
            TransitionSymbol = symbol;
        }

        public State<TAlphabet, TToken> CurrentState { get; }
        public TAlphabet TransitionSymbol { get; }
    }
}
