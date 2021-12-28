using System;

namespace EMP.Automata
{
    /// <summary>
    /// Represents single instance of for finite state machine state.
    /// </summary>
    /// <typeparam name="TInSymbol">Generic type representing symbol of input alphabet. </typeparam>
    /// <typeparam name="TOutSymbol">Generic type representing token that may be carried by state.</typeparam>
    public class State<TInSymbol, TOutSymbol>
    {
        private bool _isAcceptState;
        private bool _isTrapState;
        private TOutSymbol _output;

        /// <summary>
        /// Parameterless constructor. Returns instance of State class.
        /// </summary>
        public State()
        {
            _isAcceptState = false;
            _isTrapState = false;
            _output = default;
        }
        /// <summary>
        /// Returns instance of State class.
        /// </summary>
        /// <param name="isAccept">True if state is accept state.</param>
        /// <param name="isTrap">True if state is trap state.</param>
        /// <param name="output">Token carried by state.</param>
        public State(bool isAccept, bool isTrap, TOutSymbol output = default)
        {
            _isAcceptState = isAccept;
            _isTrapState = isTrap;
            _output = output ?? throw new ArgumentNullException();
        }
        /// <summary>
        /// Returns instance of State class.
        /// </summary>
        /// <param name="isAccept">True if state is accept state.</param>
        /// <param name="isTrap">True if state is trap state.</param>
        /// <param name="output">Token carried by state.</param>
        /// <param name="entryAction">Delegate for EtryActoin event.</param>
        /// <param name="exitAction">Delegate for ExitAction event.</param>
        public State(bool isAccept, bool isTrap, TOutSymbol output, ActionEventHandler entryAction, ActionEventHandler exitAction)
            : this(isAccept, isTrap, output)
        {
            if (entryAction is not null) EntryAction += entryAction;
            if (entryAction is not null) ExitAction += exitAction;
        }

        /// <summary>
        /// State entry action event.
        /// Event is called once upon state entrance.
        /// </summary>
        public event ActionEventHandler EntryAction;
        /// <summary>
        /// State exit action event.
        /// Event is called once after state exit.
        /// </summary>
        public event ActionEventHandler ExitAction;

        /// <summary>
        /// Returns true if state is accept state.
        /// </summary>
        public bool IsAcceptState => _isAcceptState;
        /// <summary>
        /// Returns true is state is trap state.
        /// </summary>
        public bool IsTrapState => _isTrapState;
        /// <summary>
        /// Returns output symbol carried by state.
        /// </summary>
        public TOutSymbol Output => _output;

        /// <summary>
        /// Invokes EnterAction event.
        /// </summary>
        /// <param name="symbol"></param>
        public void OnEntryAction(TInSymbol symbol)
        {
            EntryAction?.Invoke(this, new ActionEventArgs(symbol));
        }
        /// <summary>
        /// Invokes ExitAction event.
        /// </summary>
        /// <param name="symbol"></param>
        public void OnExitAction(TInSymbol symbol)
        {
            ExitAction?.Invoke(this, new ActionEventArgs(symbol));
        }
    }
}