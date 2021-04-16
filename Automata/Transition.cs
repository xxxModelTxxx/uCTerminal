using System.Collections.Generic;

namespace EMP.Automata
{
    public class Transition<TAlphabet> where TAlphabet : IEqualityComparer<TAlphabet>
    {
        public Transition(int sourceState, int targetState, TAlphabet symbol)
        {
            SourceState = sourceState;
            TargetState = targetState;
            TransitionSymbols = new HashSet<TAlphabet>(new TAlphabet[] { symbol });
        }
        public Transition(int sourceStateID, int targetStateID, IEnumerable<TAlphabet> symbols)
        {
            SourceState = sourceStateID;
            TargetState = targetStateID;
            TransitionSymbols = new HashSet<TAlphabet>(symbols);
        }

        public int SourceState { get; }
        public int TargetState { get; }
        public HashSet<TAlphabet> TransitionSymbols { get; }
    }
}