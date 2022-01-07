using System.Collections.Generic;

namespace EMP.Automata.FSM
{
    // TODO: Do rozpatrzenia czy Run() nie powinno być wywoływane w osobnym wątku lub jako async.
    // TODO: Przerobić MoveNext tak aby zwracał kolekcję State'sów. W celu implementacji w przyszłości klas NFA
    /// <summary>
    /// Provides general interface for finite state machine.
    /// </summary>
    /// <typeparam name="TSymbol">Generic type representing symbol of input alphabet. </typeparam>
    /// <typeparam name="TToken">Generic type representing token that may be carried by state.</typeparam>
    public interface IFiniteStateMachine<TSymbol, TToken>
    {
        /// <summary>
        /// Performs single computation step of finite state machine.
        /// </summary>
        /// <param name="symbol">Input transition symbol.</param>
        /// <returns></returns>
        State<TSymbol, TToken> MoveNext(TSymbol symbol);
        /// <summary>
        /// Resets finite state machine. Calling theis method causes that next step will be processed from statring state.
        /// </summary>
        void Reset();
        /// <summary>
        /// Runs complete program of finite state machine. Program is provided as a collection of transition symbols.
        /// </summary>
        /// <param name="input">Program to be performed by finite state machine represented as collection of input symbols.</param>
        /// <returns></returns>
        IEnumerable<TToken> Run(IEnumerable<TSymbol> input);
    }
}