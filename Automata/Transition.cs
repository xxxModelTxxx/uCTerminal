using System.Collections.Generic;

namespace EMP.Automata
{
    /// <summary>
    /// Represent finite state machine single transition definition class.
    /// </summary>
    /// <typeparam name="TSymbol">Generic type representing symbol of input alphabet. </typeparam>
    public class Transition<TSymbol>
    {
        /// <summary>
        /// Returns instance of Transition class.
        /// </summary>
        /// <param name="sourceState">Source state of transition.</param>
        /// <param name="targetState">Target state of transition.</param>
        /// <param name="symbol">Transition symbol.</param>
        public Transition(int sourceState, int targetState, TSymbol symbol)
        {
            SourceState = sourceState;
            TargetState = targetState;
            TransitionSymbols = new HashSet<TSymbol>(new TSymbol[] { symbol });
        }
        /// <summary>
        /// Returns instance of Transition class.
        /// </summary>
        /// <param name="sourceState">Source state of transition.</param>
        /// <param name="targetState">Target state of transition.</param>
        /// <param name="symbols">Collection of transition symbols.</param>
        public Transition(int sourceStateID, int targetStateID, IEnumerable<TSymbol> symbols)
        {
            SourceState = sourceStateID;
            TargetState = targetStateID;
            TransitionSymbols = new HashSet<TSymbol>(symbols);
        }

        /// <summary>
        /// Returns source state of transition.
        /// </summary>
        public int SourceState { get; }
        /// <summary>
        /// Returns target state of transition.
        /// </summary>
        public int TargetState { get; }
        /// <summary>
        /// Returns HashSet collection of transition symbols.
        /// </summary>
        public HashSet<TSymbol> TransitionSymbols { get; }
    }
}