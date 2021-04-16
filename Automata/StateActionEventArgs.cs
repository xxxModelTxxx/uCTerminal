using System;

namespace EMP.Automata
{
    /// <summary>
    /// EventArgs class for StateActionEventHandler delegate.
    /// </summary>
    public class StateActionEventArgs : EventArgs
    {
        /// <summary>
        /// Returns object passed with event.
        /// </summary>
        public StateActionEventArgs(object e)
        {
            Package = e;
        }

        /// <summary>
        /// Creates instance of StateActionEventArgs.
        /// </summary>
        /// <param name="e">Object passed with event.</param>
        public object Package { get; }    
    }
}