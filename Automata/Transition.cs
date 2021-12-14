using System.Collections.Generic;

namespace EMP.Automata
{
    /// <summary>
    /// Represent finite automata single transition definition class.
    /// </summary>
    /// <typeparam name="TSymbol">Generic type representing symbol of input alphabet. </typeparam>
    public class Transition<TSymbol, TToken>
    {
        /// <summary>
        /// Returns instance of Transition class.
        /// </summary>
        /// <param name="sourceState">Source state of transition.</param>
        /// <param name="targetState">Target state of transition.</param>
        /// <param name="symbol">Transition symbol.</param>
        public Transition(State<TSymbol, TToken> sourceState, State<TSymbol, TToken> targetState, TSymbol symbol)
            : this(sourceState, targetState)
        {
            InputSymbols = new List<TSymbol>(new TSymbol[] { symbol });
        }
        /// <summary>
        /// Returns instance of Transition class.
        /// </summary>
        /// <param name="sourceState">Source state of transition.</param>
        /// <param name="targetState">Target state of transition.</param>
        /// <param name="symbol">Transition symbol.</param>
        /// <param name="transitionAction">Delegate for TransitionAction event.</param>
        public Transition(State<TSymbol, TToken> sourceState, State<TSymbol, TToken> targetState, TSymbol symbol, ActionEventHandler transitionAction)
            : this(sourceState, targetState, symbol)
        {
            if (transitionAction != null) TransitionAction += transitionAction;
        }
        /// <summary>
        /// Returns instance of Transition class.
        /// </summary>
        /// <param name="sourceState">Source state of transition.</param>
        /// <param name="targetState">Target state of transition.</param>
        /// <param name="symbols">Collection of transition symbols.</param>
        public Transition(State<TSymbol, TToken> sourceState, State<TSymbol, TToken> targetState, IEnumerable<TSymbol> symbols)
            : this(sourceState, targetState)
        {
            InputSymbols = new List<TSymbol>(symbols);
        }
        /// <summary>
        /// Returns instance of Transition class.
        /// </summary>
        /// <param name="sourceState">Source state of transition.</param>
        /// <param name="targetState">Target state of transition.</param>
        /// <param name="symbols">Collection of transition symbols.</param>
        /// <param name="transitionAction">Delegate for TransitionAction event.</param>
        public Transition(State<TSymbol, TToken> sourceState, State<TSymbol, TToken> targetState, IEnumerable<TSymbol> symbols, ActionEventHandler transitionAction)
            :this(sourceState, targetState, symbols)
        {
            if (transitionAction != null) TransitionAction += transitionAction;
        }
        private Transition(State<TSymbol, TToken> sourceState, State<TSymbol, TToken> targetState)
        {
            SourceState = sourceState;
            TargetState = targetState;
        }

        /// <summary>
        /// Transition action event.
        /// Event is called once for each transition pass.
        /// </summary>
        public event ActionEventHandler TransitionAction;

        /// <summary>
        /// Returns source state of transition.
        /// </summary>
        public State<TSymbol, TToken> SourceState { get; }
        /// <summary>
        /// Returns target state of transition.
        /// </summary>
        public State<TSymbol, TToken> TargetState { get; }
        /// <summary>
        /// Returns HashSet collection of transition symbols.
        /// </summary>
        public List<TSymbol> InputSymbols { get; }

        /// <summary>
        /// Returns true if transition contains given input symbol.
        /// </summary>
        /// <param name="symbol">Symbol to be checked</param>
        /// <returns></returns>
        public bool ContainsInputSymbol(TSymbol symbol)
        {
            return InputSymbols.Contains(symbol);
        }

        /// <summary>
        /// Invokes TransitionAction event.
        /// </summary>
        /// <param name="input"></param>
        public void OnTransition(IEnumerable<TSymbol> input)
        {
            TransitionAction?.Invoke(this, new ActionEventArgs(input));
        }
    }
}