using System.Collections.Generic;

namespace EMP.Automata
{
    // TODO: Do rozpatrzenia czy Run() nie powinno być wywoływane w osobnym wątku lub jako async.
    public interface IFiniteStateMachine<TAlphabet, TToken> where TAlphabet : IEqualityComparer<TAlphabet>
    {
        State<TAlphabet, TToken> MoveNext(TAlphabet symbol);
        void Reset();
        IEnumerable<TToken> Run(IEnumerable<TAlphabet> input);
    }
}