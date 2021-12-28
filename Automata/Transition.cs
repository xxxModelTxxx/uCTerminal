using System;
using System.Collections.Generic;

namespace EMP.Automata
{
    // TODO: Dodać GetHashCode i Equals
    /// <summary>
    /// Represent finite automata single transition definition class.
    /// </summary>
    /// <typeparam name="TInSymbol">Generic type representing symbol of input alphabet. </typeparam>
    public class Transition<TInSymbol, TOutSymbol>
    {
        private List<TInSymbol> _inputSymbols;
        private State<TInSymbol, TOutSymbol> _sourceState;
        private State<TInSymbol, TOutSymbol> _targetState;

        /// <summary>
        /// Returns instance of Transition class.
        /// </summary>
        /// <param name="sourceState">Source state of transition.</param>
        /// <param name="targetState">Target state of transition.</param>
        /// <param name="symbol">Transition symbol.</param>
        public Transition(State<TInSymbol, TOutSymbol> sourceState, State<TInSymbol, TOutSymbol> targetState, TInSymbol symbol)
            : this(sourceState, targetState)
        {
            _inputSymbols = new List<TInSymbol>(new TInSymbol[] { symbol });
        }
        /// <summary>
        /// Returns instance of Transition class.
        /// </summary>
        /// <param name="sourceState">Source state of transition.</param>
        /// <param name="targetState">Target state of transition.</param>
        /// <param name="symbol">Transition symbol.</param>
        /// <param name="transitionAction">Delegate for TransitionAction event.</param>
        public Transition(State<TInSymbol, TOutSymbol> sourceState, State<TInSymbol, TOutSymbol> targetState, TInSymbol symbol, ActionEventHandler transitionAction)
            : this(sourceState, targetState, symbol)
        {
            if (transitionAction is not null) TransitionAction += transitionAction;
        }
        /// <summary>
        /// Returns instance of Transition class.
        /// </summary>
        /// <param name="sourceState">Source state of transition.</param>
        /// <param name="targetState">Target state of transition.</param>
        /// <param name="symbols">Collection of transition symbols.</param>
        public Transition(State<TInSymbol, TOutSymbol> sourceState, State<TInSymbol, TOutSymbol> targetState, ICollection<TInSymbol> symbols)
            : this(sourceState, targetState)
        {
            _inputSymbols = new List<TInSymbol>(symbols);
        }
        /// <summary>
        /// Returns instance of Transition class.
        /// </summary>
        /// <param name="sourceState">Source state of transition.</param>
        /// <param name="targetState">Target state of transition.</param>
        /// <param name="symbols">Collection of transition symbols.</param>
        /// <param name="transitionAction">Delegate for TransitionAction event.</param>
        public Transition(State<TInSymbol, TOutSymbol> sourceState, State<TInSymbol, TOutSymbol> targetState, ICollection<TInSymbol> symbols, ActionEventHandler transitionAction)
            :this(sourceState, targetState, symbols)
        {
            if (transitionAction is not null) TransitionAction += transitionAction;
        }
        private Transition(State<TInSymbol, TOutSymbol> sourceState, State<TInSymbol, TOutSymbol> targetState)
        {
            _sourceState = sourceState ?? throw new ArgumentNullException();
            _targetState = targetState ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Transition action event.
        /// Event is called once for each transition pass.
        /// </summary>
        public event ActionEventHandler TransitionAction;

        /// <summary>
        /// Returns HashSet collection of transition symbols.
        /// </summary>
        public List<TInSymbol> InputSymbols => _inputSymbols;
        /// <summary>
        /// Returns source state of transition.
        /// </summary>
        public State<TInSymbol, TOutSymbol> SourceState => _sourceState;
        /// <summary>
        /// Returns target state of transition.
        /// </summary>
        public State<TInSymbol, TOutSymbol> TargetState => _targetState;

        /// <summary>
        /// Returns true if transition contains given input symbol.
        /// </summary>
        /// <param name="symbol">Symbol to be checked</param>
        /// <returns></returns>
        public bool ContainsInputSymbol(TInSymbol symbol)
        {
            return InputSymbols.Contains(symbol);
        }

        /// <summary>
        /// Invokes TransitionAction event.
        /// </summary>
        /// <param name="symbol"></param>
        public void OnTransitionAction(TInSymbol symbol)
        {
            TransitionAction?.Invoke(this, new ActionEventArgs(symbol));
        }
    }
}