using System;
using System.Collections.Generic;
using System.Linq;

namespace EMP.Syntax.Grammar
{
    public class LL1Grammar : Grammar
    {
        private Dictionary<Symbol, Dictionary<Symbol, List<Rule>>> _parseTable;

        public LL1Grammar(ICollection<Rule> rules)
            : base(rules)
        {
            foreach (Rule r in rules)
            {
                if (r.Left.First().IsTerminal)
                {
                    throw new InvalidRuleException(r, "Invalid LL1 grammar rule. Terminal symbol used on LHS.");
                }
                if (r.Right.Count() == 1 && r.Left.First().GetHashCode() == r.Right.First().GetHashCode())
                {
                    throw new InvalidRuleException(r, "Invalid LL1 grammar rule. LHS equals RHS.");
                }
                if (r.Left.Count() != 1)
                {
                    throw new InvalidRuleException(r, "Invalid LL1 grammar rule. LHS empty or more than one LHS symbol preasent.");
                }
                if (r.Right.Count() == 0)
                {
                    throw new InvalidRuleException(r, "Invalid LL1 grammar rule. RHS not preasent.");
                }
            }
            InitializeParseTable();
        }

        public List<Rule> ParseTable(Symbol nonTerminal, Symbol terminal)
        {
            if (!terminal.IsTerminal || nonTerminal.IsTerminal)
            {
                return null;
            }
            else
            {
                return _parseTable[nonTerminal][terminal];
            }
        }
        public void PrintRules()
        {
            Console.WriteLine("List of grammar rules");
            foreach (Rule r in Rules)
            {
                Console.WriteLine("{0}", r.ToString());
            }
        }
        public void PrintSummary()
        {
            const int predictCol = 33;
            const int followCol = 75;
            var nul = GetNullableSet();
            var pre = GetPredictSet();
            var fol = GetFollowSet();

            PrintHeader();
            
            foreach (Symbol s in Symbols)
            {
                int curLine = Console.CursorTop;
                PrintSymbolGenInfo(s);
                PrintPredict(s, curLine);
                PrintFollow(s, curLine);
                Console.CursorTop = curLine + Math.Max(pre[s].Count, s.IsTerminal?0:fol[s].Count);
            }

            void PrintHeader()
            {
                Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-20} {4,-20} {5,-20}\n", "SYMBOL", "TERMINAL", "NULLABLE", "PREDICT RULE", "FIRST", "FOLLOW");
            }
            void PrintSymbolGenInfo(Symbol sym)
            {
                Console.Write("{0,-10} {1,-10} {2,-10}", sym.Name, sym.IsTerminal, nul[sym]);
            }
            void PrintPredict(Symbol sym, int line)
            {
                foreach(Predict p in pre[sym])
                {
                    Console.CursorLeft = predictCol;
                    Console.WriteLine("{0,-20} {1,-20}", p.GetRule?.ToString(), p.GetSymbol?.ToString());
                }
                Console.CursorTop = line;
            }
            void PrintFollow(Symbol sym, int line)
            {
                if (!sym.IsTerminal)
                {
                    foreach (Symbol s in fol[sym])
                    {
                        Console.CursorLeft = followCol;
                        Console.WriteLine("{0,-20}", s.ToString());
                    }
                }
                Console.CursorTop = line;
            }
        }
        private IDictionary<Symbol, ICollection<Predict>> GetPredictSet()
        {
            // Initialize
            var output = new Dictionary<Symbol, HashSet<Predict>>();
            var nullables = GetNullableSet();
            bool changed = true;
            foreach (Symbol s in Symbols)
            {
                output.Add(s, new HashSet<Predict>());
            }

            // If X is terminal, then FIRST(X) is {X}
            foreach (Symbol s in Symbols)
            {
                if (s.IsTerminal)
                {
                    output[s].Add(new Predict(null,s));
                }
            }

            // If X ::= e is a production, then add e to FIRST(X)
            foreach (Rule r in Rules)
            {
                if (r.Right.First().GetHashCode() == Symbol.Epsilon.GetHashCode())
                {
                    output[r.Left.First()].Add(new Predict(r,Symbol.Epsilon));
                }
            }

            // If X is nonterminal and X :== Y1 Y2 ... Yk. is a production, then place a in FIRST(X) if for some i, a is in
            // FIRST(Yi), and e is in all of FIRST(Y1), ... , FIRST(Yi - 1); that is, Y1, ... ,Yi - 1 :== e. If e is in FIRST(Yj) for
            // all j = 1, 2, ... , k, then add e to FIRST(X).
            while (changed)
            {
                changed = false;

                foreach (Rule r in Rules)
                {
                    bool fullNullable = true;
                    foreach (Symbol s in r.Right)
                    {
                        // Check if symbol is nullable, if true move to next RHS symbol
                        if (nullables[s])
                        {
                            continue;
                        }
                        // Add FIRSTs set of current RHS symbol to FIRSTs set of LHS symbol and escape the loop
                        else
                        {
                            foreach (Predict p in output[s])
                            {
                                changed |= output[r.Left.First()].Add(new Predict(r, p.GetSymbol));
                            }
                            fullNullable = false;
                            break;
                        }
                    }
                    // Check if all RHS symbols were nullable, if true add epsilon to FIRSTs set of LHS symbol
                    if (fullNullable)
                    {
                        changed |= output[r.Left.First()].Add(new Predict(r, Symbol.Epsilon));
                    }
                }
            }

            var result = new Dictionary<Symbol, ICollection<Predict>>();
            foreach (KeyValuePair<Symbol, HashSet<Predict>> kvp in output)
            {
                result.Add(kvp.Key, kvp.Value);
            }
            return result;
        }
        private IDictionary<Symbol, ICollection<Symbol>> GetFirstSet()
        {
            // Initialize
            var predicts = GetPredictSet();
            var result = new Dictionary<Symbol, ICollection<Symbol>>();

            // Extract FIRSTS collection from PREDICT collection
            foreach (KeyValuePair<Symbol, ICollection<Predict>> kvp in predicts)
            {
                var t = new List<Symbol>();

                foreach (Predict p in kvp.Value)
                {
                    t.Add(p.GetSymbol);
                }

                result.Add(kvp.Key, t);
            }
            return result;
        }
        private IDictionary<Symbol, ICollection<Symbol>> GetFollowSet()
        {
            // Initialize
            var output = new Dictionary<Symbol, HashSet<Symbol>>();
            var firsts = GetFirstSet();
            bool changed = true;

            foreach (Symbol s in NonTerminals)
            {
                output.Add(s, new HashSet<Symbol>());
            }

            // Place $ in FOLLOW(S), where S is the start symbol and $ is the input right endmarker.
            output[Rules.First().Left.First()].Add(Symbol.End);

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

            var result = new Dictionary<Symbol, ICollection<Symbol>>();
            foreach (KeyValuePair<Symbol, HashSet<Symbol>> kvp in output)
            {
                result.Add(kvp.Key, kvp.Value);
            }
            return result;
        }
        private IDictionary<Symbol, bool> GetNullableSet()
        {
            // initialize
            var changed = true;
            var output = new Dictionary<Symbol, bool>();
            
            // Generate collection for nullable information
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
        private void InitializeParseTable()
        {
            var nter = NonTerminals;
            var ter = Terminals;
            var pre = GetPredictSet();
            var fol = GetFollowSet();

            // Create parse table instance
            _parseTable = new Dictionary<Symbol, Dictionary<Symbol, List<Rule>>>();
            foreach (Symbol s1 in nter)
            {
                _parseTable.Add(s1, new Dictionary<Symbol, List<Rule>>());
                foreach (Symbol s2 in ter)
                {
                    if (s2 != Symbol.Epsilon)
                    {
                        _parseTable[s1].Add(s2, new List<Rule>());
                    }
                }
            }

            // Fill pasre table
            // Iterate through rows (non terminals)
            foreach (Symbol s1 in nter)
            {               
                // Iterate throug PREDICTS (FIRSTS)
                foreach (Predict p in pre[s1])
                {
                    // if current FIRST is "nil" copy current rule to all columns of terminals listed as FOLLOWs of current non terminal  
                    if (p.GetSymbol == Symbol.Epsilon)
                    {
                        foreach (Symbol s2 in fol[s1])
                        {
                            _parseTable[s1][s2].Add(p.GetRule);
                        }
                    }
                    // otherwise put current rule into current [row][column]
                    else
                    {
                        _parseTable[s1][p.GetSymbol].Add(p.GetRule);
                    }
                }
            }
        }
    }
}