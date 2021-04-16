using System;

namespace EMP.Automata
{
    public class StateActionEventArgs : EventArgs
    {
        public object Package { get; }

        public StateActionEventArgs(object e)
        {
            Package = e;
        }
}
}