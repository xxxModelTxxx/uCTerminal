using System.Collections.Generic;

namespace EMP.Automata
{
    /// <summary>
    /// Represents single instance of for finite state machine state.
    /// </summary>
    /// <typeparam name="TAlphabet">Generic type representing symbol of input alphabet. </typeparam>
    /// <typeparam name="TToken">Generic type representing token that may be carried by state.</typeparam>
    public class State<TAlphabet, TToken> where TAlphabet : IEqualityComparer<TAlphabet>
    {
        /// <summary>
        /// Parameterless constructor. Returns instance of State class.
        /// </summary>
        public State()
        {
            IsAcceptState = false;
            IsTrapState = false;
            Token = default;
        }
        /// <summary>
        /// Returns instance of State class.
        /// </summary>
        /// <param name="isAccept">True if state is accept state.</param>
        /// <param name="isTrap">True if state is trap state.</param>
        public State(bool isAccept, bool isTrap)
        {
            IsAcceptState = isAccept;
            IsTrapState = isTrap;
            Token = default;
        }
        /// <summary>
        /// Returns instance of State class.
        /// </summary>
        /// <param name="isAccept">True if state is accept state.</param>
        /// <param name="isTrap">True if state is trap state.</param>
        /// <param name="token">Token carried by state.</param>
        public State(bool isAccept, bool isTrap, TToken token)
        {
            IsAcceptState = isAccept;
            IsTrapState = isTrap;
            Token = token;
        }
        /// <summary>
        /// Returns instance of State class.
        /// </summary>
        /// <param name="isAccept">True if state is accept state.</param>
        /// <param name="isTrap">True if state is trap state.</param>
        /// <param name="token">Token carried by state.</param>
        /// <param name="entryAction">Delegate for EtryActoin event.</param>
        /// <param name="exitAction">Delegate for ExitAction event.</param>
        public State(bool isAccept, bool isTrap, TToken token, StateActionEventHandler entryAction, StateActionEventHandler exitAction)
        {
            IsAcceptState = isAccept;
            IsTrapState = isTrap;
            Token = token;
            if (entryAction != null) EntryAction += entryAction;
            if (entryAction != null) ExitAction += exitAction;
        }

        /// <summary>
        /// State entry action event.
        /// Event is called once after upon state entrance.
        /// </summary>
        public event StateActionEventHandler EntryAction;
        /// <summary>
        /// State exit action event.
        /// Event is called once after upon state exit.
        /// /// </summary>
        public event StateActionEventHandler ExitAction;

        /// <summary>
        /// Returns true if state is accept state.
        /// </summary>
        public bool IsAcceptState { get; }
        /// <summary>
        /// Returns true is state is trap state.
        /// </summary>
        public bool IsTrapState { get; }
        /// <summary>
        /// Returns token carried by state.
        /// </summary>
        public TToken Token { get; }

        /// <summary>
        /// Invokes EnterAction event.
        /// </summary>
        /// <param name="input"></param>
        public void EnterState(IEnumerable<TAlphabet> input)
        {
            EntryAction?.Invoke(this, new StateActionEventArgs(input));
        }
        /// <summary>
        /// Invokes ExitAction event.
        /// </summary>
        /// <param name="input"></param>
        public void ExitState(IEnumerable<TAlphabet> input)
        {
            ExitAction?.Invoke(this, new StateActionEventArgs(input));
        }
    }
}