using System;

namespace EMP.Automata
{
    /// <summary>
    /// Finite state machine computation options.
    /// </summary>
    [Flags]
    public enum FiniteStateMechineOption
    {
        None = 0,
        CallEntryEvent = 1,
        CallExitEvent = 2,
        ResetOnTrapState = 4,
        AbortOnTrapState = 8,
        ClearBufferOnReset = 16,
        RepeatStateIfTransitionNotFound = 32
    }
}
