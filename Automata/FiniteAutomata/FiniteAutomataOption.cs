using System;

namespace EMP.Automata.FiniteAutomata
{
    /// <summary>
    /// Finite automata computation options.
    /// </summary>
    [Flags]
    public enum FiniteAutomataOption
    {
        None = 0,
        CallEntryAction = 1,
        CallExitAction = 2,
        CallTransitionAction = 4,
        ResetOnTrapState = 8,
        AbortOnTrapState = 16,
        AbortOnAcceptState = 32,
        ClearBufferOnReset = 64,
        RepeatStateIfTransitionNotFound = 128
    }
}
