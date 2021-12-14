using System.Collections.Generic;

namespace EMP.Automata.FiniteAutomata
{
    /// <summary>
    /// Represents instance of Deterministic Finite Automata.
    /// </summary>
    /// <typeparam name="TSymbol">Generic type representing symbol of input alphabet. </typeparam>
    /// <typeparam name="TToken">Generic type representing token that may be carried by state.</typeparam>
    public class DFA<TSymbol, TToken> : IFiniteAutomata<TSymbol, TToken>
    {
        /// <summary>
        /// Internal buffer for already processed and current input symbols.
        /// </summary>
        private List<TSymbol> _inputBuffer;
        /// <summary>
        /// Reference to current state.
        /// </summary>
        private State<TSymbol, TToken> _currentState;
        /// <summary>
        /// Reference to start state.
        /// </summary>
        private State<TSymbol, TToken> _startState;
        /// <summary>
        /// State-Transition table
        /// </summary>
        private Dictionary<State<TSymbol, TToken>, Dictionary<TSymbol, Transition<TSymbol, TToken>>> _stateTransitionTable;
        /// <summary>
        /// DFA options.
        /// </summary>
        private FiniteAutomataOption _options;

        /// <summary>
        /// Returns instance of DFA.
        /// </summary>
        /// <param name="states">Collection of DFA states.</param>
        /// <param name="transitions">Collection of DFA transitions.</param>
        /// <param name="options">DFA options.</param>
        public DFA(IEnumerable<Transition<TSymbol, TToken>> transitions,
                    State<TSymbol, TToken> startState,
                    FiniteAutomataOption options = FiniteAutomataOption.None)
        {
            _inputBuffer = new List<TSymbol>();
            _startState = startState;
            _currentState = _startState;
            _options = options;
        }

        private void FillStateTransitionTable(IEnumerable<Transition<TSymbol, TToken>> transitions)
        {
            var states = new HashSet<State<TSymbol, TToken>>();
            var symbols = new HashSet<TSymbol>();
            
            // Create State-Transition Table
            _stateTransitionTable = new Dictionary<State<TSymbol, TToken>, Dictionary<TSymbol, Transition<TSymbol, TToken>>>();

            // Extract states and symbols from transitions
            foreach (Transition<TSymbol, TToken> t in transitions)
            {
                states.Add(t.SourceState);
                states.Add(t.TargetState);
                symbols.UnionWith(t.InputSymbols);
            }

            // Fill State-Transition Table rows
            foreach (State<TSymbol, TToken> s in states)
            {
                _stateTransitionTable.Add(s, new Dictionary<TSymbol, Transition<TSymbol, TToken>>());
            }

            // Fill State-Transition Table
            // UWAGA: tworzy wyłącznie kolumny tam gdzie są określone przejścia. W przypadku ich braku będzie NULL - sprawdzić zachowanie
            foreach (Transition<TSymbol, TToken> t in transitions)
            {
                foreach (TSymbol s in t.InputSymbols)
                {
                    _stateTransitionTable[t.TargetState].Add(s, t);
                }
            }
        }

        /// <summary>
        /// Performs single computation step of finite state machine.
        /// </summary>
        /// <param name="symbol">Input transition symbol.</param>
        /// <returns></returns>
        public State<TSymbol, TToken> MoveNext(TSymbol symbol)
        {
            _inputBuffer.Add(symbol);

            foreach (Transition<TSymbol> tr in _transitions)
            {
                if (tr.SourceState == _counter && tr.TransitionSymbols.Contains(symbol))
                {
                    if (_counter != tr.TargetState)
                    {
                        TryCallExitAction();
                        _counter = tr.TargetState;
                        TryCallEntryAction();
                    }
                    return _states[_counter];
                }
            }

            return TryTerminateInvalidControlFlow();
        }
        /// <summary>
        /// Resets DFA. Calling this method resets internal program counter (next step will be processed from statring state) and clears internal buffer if ClearBufferOnReset option is eneabled.
        /// </summary>
        public void Reset()
        {
            _currentState = _startState;
            TryResetBuffer();
        }
        /// <summary>
        /// Runs complete program of DFA. Program is provided as a collection of transition symbols.
        /// </summary>
        /// <param name="input">Program to be performed by finite state machine represented as collection of input symbols.</param>
        /// <returns></returns>
        public IEnumerable<TToken> Run(IEnumerable<TSymbol> input)
        {
            _inputBuffer.Clear();
            _counter = 0;

            IList<TToken> _tokens = new List<TToken>();

            foreach (TSymbol a in input)
            {
                var st = MoveNext(a);

                if (st.IsAcceptState)
                {
                    _tokens.Add(st.Token);
                    Reset();
                }
                else if (st.IsTrapState)
                {
                    TryEscapeTrapState();
                }
            }

            return _tokens;
        }
        /// <summary>
        /// Performs attempt to reset DFA. Reset is performed if ResetOnTrapState option is eneabled.
        /// </summary>
        private void TryEscapeTrapState()
        {
            if ((_options & FiniteAutomataOption.ResetOnTrapState) != 0)
            {
                Reset();
            }
        }
        /// <summary>
        /// Performs attempt of invoking EntryAction event for current state. Attempt is succesful if CallEntryAction option is eneabled.
        /// </summary>
        private void TryCallEntryAction()
        {
            if ((_options & FiniteAutomataOption.CallEntryAction) != 0)
            {
                _states[_counter].OnEntryAction(_inputBuffer);
            }
        }
        /// <summary>
        /// Performs attempt of invoking ExitAction event for current state. Attempt is succesful if CallExitAction option is eneabled.
        /// </summary>
        private void TryCallExitAction()
        {
            if ((_options & FiniteAutomataOption.CallExitAction) != 0)
            {
                _states[_counter].OnExitAction(_inputBuffer);
            }
        }
        /// <summary>
        /// Performs attempt of invoking TransitionAction event for current transition. Attempt is succesful if CallTransitionAction option is eneabled.
        /// </summary>
        private void TryCallTransitionAction()
        {
            if ((_options & FiniteAutomataOption.CallTransitionAction) != 0)
            {
                _states[_counter].OnTransitionAction(_inputBuffer);
            }
        }
        /// <summary>
        /// Performs attempt to reset internal input buffer. Reset is performed if ClearBufferOnReset option is eneabled.
        /// </summary>
        private void TryResetBuffer()
        {
            if ((_options & FiniteAutomataOption.ClearBufferOnReset) != 0)
            {
                _inputBuffer.Clear();
            }
        }
        /// <summary>
        /// Performs attempt of restoring control flow after invalid input symbol or lack of relevant transition. 
        /// If RepeatStateIfTransitionNotFound option is eneabled, program is contiued from current state (if next input symbol is prvided). Othervise throws InvalidControlFlowException exception.
        /// </summary>
        /// <returns></returns>
        private State<TSymbol, TToken> TryTerminateInvalidControlFlow()
        {
            if ((_options & FiniteAutomataOption.RepeatStateIfTransitionNotFound) != 0)
            {
                return _states[_counter];
            }
            else
            {
                throw new InvalidControlFlowException<TSymbol, TToken>(_states[_counter], _inputBuffer[^1]);
            }
        }
    }
}