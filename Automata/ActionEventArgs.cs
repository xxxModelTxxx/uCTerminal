using System;

namespace EMP.Automata
{
    /// <summary>
    /// EventArgs class for StateActionEventHandler delegate.
    /// </summary>
    public class ActionEventArgs : EventArgs
    {
        /// <summary>
        /// Returns object passed with event.
        /// </summary>
        public ActionEventArgs(object e)
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