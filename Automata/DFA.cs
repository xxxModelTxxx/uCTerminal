using System.Collections.Generic;

namespace EMP.Automata
{
    public class DFA<TAlphabet, TToken> : IFiniteStateMachine<TAlphabet, TToken> where TAlphabet : IEqualityComparer<TAlphabet>
    {
        // TODO: Wymyślić bardziej adekwatną nazwę dla _buffer
        private List<TAlphabet> _buffer;
        private int _counter;        
        private IDictionary<int, State<TAlphabet, TToken>> _states;
        private IEnumerable<Transition<TAlphabet>> _transitions;
        private FiniteStateMechineOption _options;

        // TODO: Zweryfikować czy da się zweryfikować czy nie ma duplikatów w _transitions;
        public DFA( IDictionary<int, State<TAlphabet, TToken>> states, 
                    IEnumerable<Transition<TAlphabet>> transitions, 
                    FiniteStateMechineOption options = FiniteStateMechineOption.None)
        {
            _buffer = new List<TAlphabet>();
            _counter = 0;
            _states = states;
            _transitions = transitions;
            _options = options;
        }

        public State<TAlphabet, TToken> MoveNext(TAlphabet symbol)
        {
            _buffer.Add(symbol);

            foreach(Transition<TAlphabet> tr in _transitions)
            {
                if(tr.SourceState == _counter && tr.TransitionSymbols.Contains(symbol))
                {
                    TryCallExitEvent();
                    _counter = tr.TargetState;
                    TryCallEntryEvent();
                    return _states[_counter];
                }
            }

            return TryTerminateInvalidControlFlow();
        }
        public void Reset()
        {
            _counter = 0;
            TryResetBuffer();
        }
        public IEnumerable<TToken> Run(IEnumerable<TAlphabet> input)
        {
            _buffer.Clear();
            _counter = 0;

            IList<TToken> _tokens = new List<TToken>();

            foreach (TAlphabet a in input)
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
        private void TryEscapeTrapState()
        {
            if ((_options & FiniteStateMechineOption.ResetOnTrapState) != 0)
            {
                Reset();
            }
        }
        private void TryCallEntryEvent()
        {
            if ((_options & FiniteStateMechineOption.CallEntryEvent) != 0)
            {
                _states[_counter].EnterState(_buffer);
            }
        }
        private void TryCallExitEvent()
        {
            if ((_options & FiniteStateMechineOption.CallExitEvent) != 0)
            {
                _states[_counter].ExitState(_buffer);
            }
        }
        private void TryResetBuffer()
        {
            if ((_options & FiniteStateMechineOption.ClearBufferOnReset) != 0)
            {
                _buffer.Clear();
            }
        }
        private State<TAlphabet, TToken> TryTerminateInvalidControlFlow()
        {
            if ((_options & FiniteStateMechineOption.RepeatStateIfTransitionNotFound) != 0)
            {
                return _states[_counter];
            }
            else
            {
                throw new InvalidControlFlowException<TAlphabet, TToken>(_states[_counter], _buffer[^1]);
            }
        }
    }
}