using System;
using System.Collections.Generic;

namespace EMP.Automata.FiniteAutomata
{
    // TODO: Check transition collection passed to contructor whether they are deterministic - implement GetHashCode and Equals
    /// <summary>
    /// Represents instance of Deterministic Finite Automata.
    /// </summary>
    /// <typeparam name="TInSymbol">Generic type representing symbol of input alphabet. </typeparam>
    /// <typeparam name="TOutSymbol">Generic type representing token that may be carried by state.</typeparam>
    public class DFA<TInSymbol, TOutSymbol> : IFiniteAutomata<TInSymbol, TOutSymbol>
    {
        private State<TInSymbol, TOutSymbol> _currentState;
        private State<TInSymbol, TOutSymbol> _errorState;
        private State<TInSymbol, TOutSymbol> _startState;
        private AutomataStatus _status;
        private FiniteAutomataOptions _options;
        private IEnumerable<Transition<TInSymbol, TOutSymbol>> _transitions;

        /// <summary>
        /// Returns instance of DFA.
        /// </summary>
        /// <param name="transitions">Collection of DFA transitions.</param>
        /// <param name="startState">Reference to start state.</param>
        /// <param name="errorState">Reference to error state.</param>
        /// <param name="options">DFA options.</param>
        public DFA(IEnumerable<Transition<TInSymbol, TOutSymbol>> transitions,
                    State<TInSymbol, TOutSymbol> startState,
                    State<TInSymbol, TOutSymbol> errorState,
                    FiniteAutomataOptions options = FiniteAutomataOptions.None)
        {
            _transitions = transitions ?? throw new ArgumentNullException();
            _startState = startState ?? throw new ArgumentNullException();
            _errorState = errorState ?? throw new ArgumentNullException();
            _currentState = _startState;
            _options = options;
            _status = AutomataStatus.Ready;
            if (!IsDeterministic()) throw new ArgumentOutOfRangeException("Non deterministic transitions passed to DFA class constructor");
        }

        /// <summary>
        /// Reads current status of DFA.
        /// </summary>
        public AutomataStatus Status => _status;
        /// <summary>
        /// Returns Transitions collection.
        /// </summary>
        public IEnumerable<Transition<TInSymbol, TOutSymbol>> Transitions => _transitions;

        /// <summary>
        /// Returns Transition transition for given state and input symbol
        /// </summary>
        /// <param name="state">Transition source state</param>
        /// <param name="inputSymbol">Transition input symbol</param>
        /// <returns></returns>
        public Transition<TInSymbol, TOutSymbol> GetTransition(State<TInSymbol, TOutSymbol> state, TInSymbol inputSymbol)
        {
            foreach (Transition<TInSymbol, TOutSymbol> t in _transitions)
            {
                if (t.SourceState == state && t.ContainsInputSymbol(inputSymbol)) return t;
            }
            return null;
        }
        /// <summary>
        /// Return collection of InputSymbols
        /// </summary>
        /// <returns></returns>
        public ICollection<TInSymbol> InputSymbols()
        {
            var symbols = new HashSet<TInSymbol>();
            foreach (Transition<TInSymbol, TOutSymbol> t in _transitions)
            {
                symbols.UnionWith(t.InputSymbols);
            }
            return symbols;
        }
        /// <summary>
        /// Performs single computation step of finite state machine.
        /// </summary>
        /// <param name="inputSymbol">Input transition symbol.</param>
        /// <returns></returns>
        public State<TInSymbol, TOutSymbol> MoveNext(TInSymbol inputSymbol)
        {
            Transition<TInSymbol, TOutSymbol> t;
            
            _status = AutomataStatus.Running;

            // Check if symbol is null
            if (inputSymbol is null)
            {
                SetError();
                throw new ArgumentNullException();
            }
            // Check if input symbol is invalid and proceed accordingly
            else if (!InputSymbols().Contains(inputSymbol))
            {
                return TryHandleInvalidInput();
            }
            // Check if transition function exist and proceed accordingly
            else if ((t = GetTransition(_currentState, inputSymbol)) is null)
            {
                return TryHandleMissingTransition();
            }
            // Process valid program step
            else
            {
                TryCallExitAction(_currentState, inputSymbol);
                TryCallTransitionAction(t, inputSymbol);
                _currentState = t.TargetState;
                TryCallEntryAction(_currentState, inputSymbol);
                return _currentState;
            }
        }
        /// <summary>
        /// Return collection of OutputSymbols
        /// </summary>
        /// <returns></returns>
        public ICollection<TOutSymbol> OutputSymbols()
        {
            var symbols = new HashSet<TOutSymbol>();
            foreach (State<TInSymbol, TOutSymbol> s in States())
            {
                symbols.Add(s.Output);
            }
            return symbols;
        }
        /// <summary>
        /// Resets DFA. Calling this method resets internal program counter (next step will be processed from statring state) and clears internal buffer if ClearBufferOnReset option is eneabled.
        /// </summary>
        public void Reset()
        {
            _status = AutomataStatus.Ready;
            _currentState = _startState;
        }
        /// <summary>
        /// Runs complete program of DFA. Program is provided as a collection of transition symbols.
        /// </summary>
        /// <param name="input">Program to be performed by finite state machine represented as collection of input symbols.</param>
        /// <returns></returns>
        public IEnumerable<TOutSymbol> Run(IEnumerable<TInSymbol> input)
        {
            if (input is null) throw new ArgumentNullException();

            IList<TOutSymbol> output = new List<TOutSymbol>();
            State<TInSymbol, TOutSymbol> st;

            Reset();

            foreach (TInSymbol sy in input)
            {
                st = MoveNext(sy);

                if (st.IsAcceptState)
                {
                    output.Add(st.Output);
                    Reset();
                }
                else if (st == _errorState)
                {
                    output.Add(st.Output);
                    break;
                }
                else if (st.IsTrapState)
                {
                    TryEscapeTrapState();
                }
            }
            return output;
        }
        /// <summary>
        /// Returns collection of States
        /// </summary>
        /// <returns></returns>
        public ICollection<State<TInSymbol, TOutSymbol>> States()
        {
            var states = new HashSet<State<TInSymbol, TOutSymbol>>();
            foreach (Transition<TInSymbol, TOutSymbol> t in _transitions)
            {
                states.Add(t.SourceState);
                states.Add(t.TargetState);
            }
            return states;
        }
        /// <summary>
        /// Sets DFA to error state.
        /// </summary>
        private void SetError()
        {
            _status = AutomataStatus.Error;
            _currentState = _errorState;
        }
        /// <summary>
        /// Performs attempt to reset DFA. Reset is performed if ResetOnTrapState option is eneabled.
        /// </summary>
        private void TryEscapeTrapState()
        {
            if ((_options & FiniteAutomataOptions.ResetOnTrapState) != 0)
            {
                Reset();
            }
            else
            {
                _status = AutomataStatus.Trapped;
            }
        }
        /// <summary>
        /// Performs attempt of invoking EntryAction event for current state. Attempt is succesful if CallEntryAction option is eneabled.
        /// </summary>
        private void TryCallEntryAction(State<TInSymbol, TOutSymbol> state, TInSymbol symbol)
        {
            if ((_options & FiniteAutomataOptions.CallEntryAction) != 0)
            {
                state.OnEntryAction(symbol);
            }
        }
        /// <summary>
        /// Performs attempt of invoking ExitAction event for current state. Attempt is succesful if CallExitAction option is eneabled.
        /// </summary>
        private void TryCallExitAction(State<TInSymbol, TOutSymbol> state, TInSymbol symbol)
        {
            if ((_options & FiniteAutomataOptions.CallExitAction) != 0)
            {
                state.OnExitAction(symbol);
            }
        }
        /// <summary>
        /// Performs attempt of invoking TransitionAction event for current transition. Attempt is succesful if CallTransitionAction option is eneabled.
        /// </summary>
        private void TryCallTransitionAction(Transition<TInSymbol, TOutSymbol> transition, TInSymbol symbol)
        {
            if ((_options & FiniteAutomataOptions.CallTransitionAction) != 0)
            {
                transition.OnTransitionAction(symbol);
            }
        }
        /// <summary>
        /// Performs attempt of restoring control flow after invalid input symbol or lack of relevant transition. 
        /// If RepeatStateIfTransitionNotFound option is eneabled, program is contiued from current state (if next input symbol is prvided). Othervise throws InvalidControlFlowException exception.
        /// </summary>
        /// <returns></returns>
        private State<TInSymbol, TOutSymbol> TryHandleInvalidInput()
        {
            if ((_options & FiniteAutomataOptions.IgnoreInvalidSymbol) != 0)
            {
                return _currentState;
            }
            else if ((_options & FiniteAutomataOptions.ResetOnInvalidSymbol) != 0)
            {
                Reset();
                return _currentState;
            }
            else
            {
                SetError();
                return _currentState;
            }
        }
        /// <summary>
        /// Performs attempt of restoring control flow after invalid input symbol or lack of relevant transition. 
        /// If RepeatStateIfTransitionNotFound option is eneabled, program is contiued from current state (if next input symbol is prvided). Othervise throws InvalidControlFlowException exception.
        /// </summary>
        /// <returns></returns>
        private State<TInSymbol, TOutSymbol> TryHandleMissingTransition()
        {
            if ((_options & FiniteAutomataOptions.IgnoreMissingTransition) != 0)
            {
                return _currentState;
            }
            else if ((_options & FiniteAutomataOptions.ResetOnMissingTransition) != 0)
            {
                Reset();
                return _currentState;
            }
            else
            {
                SetError();
                return _currentState;
            }
        }
        /// <summary>
        /// Verifies if DFA is determinnistic
        /// </summary>
        /// <returns>Returns true if DFA is deterministic or false if not.</returns>
        private bool IsDeterministic()
        {
            foreach (Transition<TInSymbol, TOutSymbol> t1 in _transitions)
            {
                foreach (Transition<TInSymbol, TOutSymbol> t2 in _transitions)
                {
                    if (t1.SourceState == t2.SourceState)
                    {
                        var s1 = new HashSet<TInSymbol>(t1.InputSymbols);
                        var s2 = new HashSet<TInSymbol>(t2.InputSymbols);

                        if (s1.Overlaps(s2)) return false;
                    }
                }
            }
            return true;
        }
    }
}