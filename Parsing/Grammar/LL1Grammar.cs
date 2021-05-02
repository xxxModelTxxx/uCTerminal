using System;
using System.Collections.Generic;
using System.Linq;

namespace EMP.Syntax.Grammar
{
    public class LL1Grammar : Grammar
    {
        private Dictionary<int, Dictionary<int, Rule>> _parseTable;

        public LL1Grammar(IList<Rule> rules)
            : base(rules)
        { }

        public Rule ParseTable(Symbol terminal, Symbol nonTerminal)
        {
            if (!terminal.IsTerminal || nonTerminal.IsTerminal)
            {
                return null;
            }
            else
            {
                return _parseTable[terminal.GetHashCode()][nonTerminal.GetHashCode()];
            }
        }
        private IEnumerable<Symbol> Firsts(Symbol symbol)
        {
            var output = new Dictionary<int, Symbol>();

            if (symbol.IsTerminal)
            {
                output.Add(symbol.GetHashCode(), symbol);
            }
            else
            {
                foreach (Rule r in Rules)
                {
                    if (symbol.GetHashCode() == r.Left.First().GetHashCode())
                    {
                        Symbol s1 = r.Right.First();
                        if (s1.IsTerminal)
                        {
                            output.TryAdd(s1.GetHashCode(), s1);
                        }
                        else
                        {
                            foreach (Symbol s2 in Firsts(s1))
                            {
                                output.TryAdd(s2.GetHashCode(), s2);
                            }
                        }
                    }
                }
            }
            return output.Values;
        }
        private IEnumerable<Symbol> Follows(Symbol symbol)
        {
            throw new NotImplementedException();
        }
        private bool IsNullabe(Symbol symbol)
        {
            var changed = true;
            var nullables = new Dictionary<int, bool>();
            
            // Create collection of nullable
            foreach(Symbol s in Symbols)
            {
                nullables.Add(s.GetHashCode(), false);
            }

            // Check for rules containig only epsilon on RHS
            foreach (Rule r in Rules)
            {
                if (r.Right.First().GetHashCode() == Symbol.Epsilon.GetHashCode())
                {
                    nullables[r.Left.First().GetHashCode()] = true;
                }
            }

            // Check (iterate) for rules containing only nullable symbols on RHS
            while (changed)
            {
                changed = false;
                foreach(Rule r in Rules)
                {
                    if (!nullables[r.Left.First().GetHashCode()])
                    {
                        bool state = true;
                        foreach (Symbol s in r.Right)
                        {
                            state &= nullables[s.GetHashCode()];
                        }
                        if (state)
                        {
                            changed = true;
                            nullables[r.Left.First().GetHashCode()] = state;
                        }
                    }
                }
            }

            // Return result
            return nullables[symbol.GetHashCode()];
        }
    }
}
