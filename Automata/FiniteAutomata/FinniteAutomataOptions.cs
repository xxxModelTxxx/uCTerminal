using System;

namespace EMP.Automata.FiniteAutomata
{
    /// <summary>
    /// Finite automata computation options.
    /// </summary>
    [Flags]
    public enum FinniteAutomataOptions
    {
        None = 0,
        CallInputAction = 1,
        CallEntryAction = 2,
        CallExitAction = 4,
        CallTransitionAction = 8,
        ResetOnTrapState = 16,
        IgnoreMissingTransition = 128,
        ResetOnMissingTransition = 256,
        IgnoreInvalidSymbol = 512,
        ResetOnInvalidSymbol = 1024
    }
}
