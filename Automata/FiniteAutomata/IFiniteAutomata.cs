using System.Collections.Generic;

namespace EMP.Automata.FiniteAutomata
{
    // TODO: Do rozpatrzenia czy Run() nie powinno być wywoływane w osobnym wątku lub jako async.
    // TODO: Przerobić na klasy virtualne tak aby można było aimplementować dowolne maszyny
    // TODO: Przerobić TSymbol na input a TToken na output
    /// <summary>
    /// Provides general interface for finite automata.
    /// </summary>
    /// <typeparam name="TSymbol">Generic type representing symbol of input alphabet. </typeparam>
    /// <typeparam name="TToken">Generic type representing token that may be carried by state.</typeparam>
    public interface IFiniteAutomata<TInSymbol, TOutSymbol>
    {
        /// <summary>
        /// Reads current status of automata.
        /// </summary>
        AutomataStatus Status { get; }

        /// <summary>
        /// Performs single computation step of finite automata.
        /// </summary>
        /// <param name="symbol">Input transition symbol.</param>
        /// <returns></returns>
        State<TInSymbol, TOutSymbol> MoveNext(TInSymbol symbol);
        /// <summary>
        /// Resets finite automata. Calling theis method causes that next step will be processed from statring state.
        /// </summary>
        void Reset();
        /// <summary>
        /// Runs complete program of finite automata. Program is provided as a collection of transition symbols.
        /// </summary>
        /// <param name="input">Program to be performed by finite state machine represented as collection of input symbols.</param>
        /// <returns></returns>
        IEnumerable<TOutSymbol> Run(IEnumerable<TInSymbol> input);
    }
}