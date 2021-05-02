using System.Collections.Generic;

namespace EMP.Automata.FSM
{
    /// <summary>
    /// Represents instance of Deterministic Finite Automata.
    /// </summary>
    /// <typeparam name="TSymbol">Generic type representing symbol of input alphabet. </typeparam>
    /// <typeparam name="TToken">Generic type representing token that may be carried by state.</typeparam>
    public class DFA<TSymbol, TToken> : IFiniteStateMachine<TSymbol, TToken>
    {
        /// <summary>
        /// Internal buffer for already processed and current input symbols.
        /// </summary>
        private List<TSymbol> _inputBuffer;
        /// <summary>
        /// Represents internal program counter. Points at current state.
        /// </summary>
        private int _counter;
        /// <summary>
        /// Colection of DFA states.
        /// </summary>
        private IDictionary<int, State<TSymbol, TToken>> _states;
        /// <summary>
        /// Collection of DFA transitions.
        /// </summary>
        private IEnumerable<Transition<TSymbol>> _transitions;
        /// <summary>
        /// DFA options.
        /// </summary>
        private FiniteStateMechineOption _options;

        // TODO: Zastanowić się czy da się zweryfikować czy nie ma duplikatów w _transitions;
        /// <summary>
        /// Returns instance of DFA.
        /// </summary>
        /// <param name="states">Collection of DFA states.</param>
        /// <param name="transitions">Collection of DFA transitions.</param>
        /// <param name="options">DFA options.</param>
        public DFA(IDictionary<int, State<TSymbol, TToken>> states,
                    IEnumerable<Transition<TSymbol>> transitions,
                    FiniteStateMechineOption options = FiniteStateMechineOption.None)
        {
            _inputBuffer = new List<TSymbol>();
            _counter = 0;
            _states = states;
            _transitions = transitions;
            _options = options;
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
                        TryCallExitEvent();
                        _counter = tr.TargetState;
                        TryCallEntryEvent();
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
            _counter = 0;
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
                var ot = MoveNext(a);

                if (ot.IsAcceptState)
                {
                    _tokens.Add(ot.Token);
                    Reset();
                }
                else if (ot.IsTrapState)
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
            if ((_options & FiniteStateMechineOption.ResetOnTrapState) != 0)
            {
                Reset();
            }
        }
        /// <summary>
        /// Performs attempt of invoking EntryEvent for current state. Attempt is succesful if CallEntryEvent option is eneabled.
        /// </summary>
        private void TryCallEntryEvent()
        {
            if ((_options & FiniteStateMechineOption.CallEntryEvent) != 0)
            {
                _states[_counter].EnterState(_inputBuffer);
            }
        }
        /// <summary>
        /// Performs attempt of invoking ExitEvent for current state. Attempt is succesful if CallExitEvent option is eneabled.
        /// </summary>
        private void TryCallExitEvent()
        {
            if ((_options & FiniteStateMechineOption.CallExitEvent) != 0)
            {
                _states[_counter].ExitState(_inputBuffer);
            }
        }
        /// <summary>
        /// Performs attempt to reset internal input buffer. Reset is performed if ClearBufferOnReset option is eneabled.
        /// </summary>
        private void TryResetBuffer()
        {
            if ((_options & FiniteStateMechineOption.ClearBufferOnReset) != 0)
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
            if ((_options & FiniteStateMechineOption.RepeatStateIfTransitionNotFound) != 0)
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