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
        private bool IsNullable(Symbol symbol)
        {
            return Nullable()[symbol];
        }
        private IDictionary<Symbol, ICollection<Symbol>> First()
        {
            // Initialize
            var output = new Dictionary<Symbol, HashSet<Symbol>>();
            var nullables = Nullable();
            bool changed = true;
            foreach (Symbol s in Symbols)
            {
                output.Add(s, new HashSet<Symbol>());
            }

            // If X is terminal, then FIRST(X) is {X}
            foreach (Symbol s in Symbols)
            {
                if (s.IsTerminal)
                {
                    output[s].Add(s);
                }
            }

            // If X ::= e is a production, then add e to FIRST(X)
            foreach (Rule r in Rules)
            {
                if (r.Right.First().GetHashCode() == Symbol.Epsilon.GetHashCode())
                {
                    output[r.Left.First()].Add(Symbol.Epsilon);
                }
            }

            // If X is nonterminal and X :== Y1 Y2 ... Yk. is a production, then place a in FIRST(X) if for some i, a is in
            // FIRST(Yi), and e is in all of FIRST(Y1), ... , FIRST(Yi - 1); that is, Y1, ... ,Yi - 1 Þ e. If e is in FIRST(Yj) for
            // all j = 1, 2, ... , k, then add e to FIRST(X).
            while (changed)
            {
                changed = false;

                foreach (Rule r in Rules)
                {
                    foreach (Symbol s1 in r.Right)
                    {
                        if (s1.IsTerminal)
                        {
                            changed = output[r.Left.First()].Add(s1);
                            break;
                        }
                        else if (nullables[s1])
                        {
                            foreach (Symbol s2 in output[s1])
                            {
                                if (s2.GetHashCode() != Symbol.Epsilon.GetHashCode())
                                {
                                    changed |= output[r.Left.First()].Add(s2);
                                }
                            }
                        }
                        else
                        {
                            foreach (Symbol s2 in output[s1])
                            {
                                changed |= output[r.Left.First()].Add(s2);
                            }
                            break;
                        }
                    }
                }
            }

            return (IDictionary <Symbol, ICollection<Symbol>>)output;
        }
        private IDictionary<Symbol, ICollection<Symbol>> Follow()
        {
            // Initialize
            var output = new Dictionary<Symbol, HashSet<Symbol>>();
            var firsts = First();
            bool changed = true;
            foreach (Symbol s in NonTerminals)
            {
                output.Add(s, new HashSet<Symbol>());
            }

            // Place $ in FOLLOW(S), where S is the start symbol and $ is the input right endmarker.
            output[Symbol.Start].Add(Symbol.End);

            // If there is a production A ::= aBb, then everything in FIRST(b), except for e, is placed in FOLLOW(B).
            foreach (Rule r in Rules)
            {
                Symbol sp = null;
                foreach (Symbol sc in r.Right)
                {
                    if (sp != null)
                    {
                        foreach (Symbol s in firsts[sc])
                        {
                            if (s != Symbol.Epsilon)
                            {
                                output[sp].Add(s);
                            }
                        }
                    }
                    if (!sc.IsTerminal)
                    {
                        sp = sc;
                    }
                    else
                    {
                        sp = null;
                    }
                }
            }

            // If there is a production A ::= aB, or a production A ::= aBb where FIRST(b) contains e (i.e., b ::= e),
            // then everything in FOLLOW(A) is in FOLLOW(B).
            while(changed)
            {
                changed = false;
                Symbol sp = null;
                foreach (Rule r in Rules)
                {
                    foreach (Symbol sc in r.Right)
                    {
                        if (sp != null && firsts[sc].Contains(Symbol.Epsilon))
                        {
                            foreach(Symbol s in output[r.Left.First()])
                            {
                                changed |= output[sp].Add(s);
                            }
                        }
                        if (!sc.IsTerminal)
                        {
                            sp = sc;
                        }
                        else
                        {
                            sp = null;
                        }
                    }
                    if (!r.Right.Last().IsTerminal)
                    {
                        foreach (Symbol s in output[r.Left.First()])
                        {
                            changed |= output[sp].Add(s);
                        }
                    }
                }
            }

            return (IDictionary<Symbol, ICollection<Symbol>>)output;

        }
        private IDictionary<Symbol, bool> Nullable()
        {
            var changed = true;
            var output = new Dictionary<Symbol, bool>();
            
            // Create collection of nullable
            foreach(Symbol s in Symbols)
            {
                output.Add(s, false);
            }

            // Check for rules containig only epsilon on RHS
            foreach (Rule r in Rules)
            {
                if (r.Right.First().GetHashCode() == Symbol.Epsilon.GetHashCode())
                {
                    output[r.Left.First()] = true;
                }
            }

            // Check (iterate) for rules containing only nullable symbols on RHS
            while (changed)
            {
                changed = false;
                foreach(Rule r in Rules)
                {
                    if (!output[r.Left.First()])
                    {
                        bool state = true;
                        foreach (Symbol s in r.Right)
                        {
                            state &= output[s];
                        }
                        if (state)
                        {
                            changed = true;
                            output[r.Left.First()] = state;
                        }
                    }
                }
            }

            // Return result
            return output;
        }
    }
}
