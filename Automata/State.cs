using System.Collections.Generic;

namespace EMP.Automata
{
    public class State<TAlphabet, TToken> where TAlphabet : IEqualityComparer<TAlphabet>
    {
        public State()
        {
            IsAcceptState = false;
            IsTrapState = false;
            Token = default;
        }
        public State(bool isAccept, bool isTrap)
        {
            IsAcceptState = isAccept;
            IsTrapState = isTrap;
            Token = default;
        }
        public State(bool isAccept, bool isTrap, TToken token)
        {
            IsAcceptState = isAccept;
            IsTrapState = isTrap;
            Token = token;
        }
        public State(bool isAccept, bool isTrap, TToken token, StateActionEventHandler entryAction, StateActionEventHandler exitAction)
        {
            IsAcceptState = isAccept;
            IsTrapState = isTrap;
            Token = token;
            if (entryAction != null) EntryAction += entryAction;
            if (entryAction != null) ExitAction += exitAction;
        }

        public event StateActionEventHandler EntryAction;
        public event StateActionEventHandler ExitAction;

        public bool IsAcceptState { get; }
        public bool IsTrapState { get; }
        public TToken Token { get; }

        public void EnterState(IEnumerable<TAlphabet> input)
        {
            EntryAction?.Invoke(this, new StateActionEventArgs(input));
        }
        public void ExitState(IEnumerable<TAlphabet> input)
        {
            ExitAction?.Invoke(this, new StateActionEventArgs(input));
        }
    }
}