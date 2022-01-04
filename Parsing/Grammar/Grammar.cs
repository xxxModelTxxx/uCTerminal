using System.Collections.Generic;

namespace EMP.Syntax.Grammar
{
    public abstract class Grammar
    {
        private ICollection<Rule> _rules;

        public Grammar(ICollection<Rule> rules)
        {
            _rules = rules;
        }

        public IEnumerable<Rule> Rules 
        { 
            get => _rules; 
        }
        public IEnumerable<Symbol> Symbols
        {
            get
            {
                var output = new List<Symbol>();

                output.AddRange(Terminals);
                output.AddRange(NonTerminals);

                return output;
            }
        }
        public IEnumerable<Symbol> Terminals 
        { 
            get
            {
                var output = new Dictionary<int, Symbol>();
                
                foreach(Rule r in Rules)
                {
                    foreach (Symbol s in r.Left)
                    {
                        if (s.IsTerminal)
                        {
                            output.TryAdd(s.GetHashCode(), s);
                        }
                    }
                    foreach (Symbol s in r.Right)
                    {
                        if (s.IsTerminal)
                        {
                            output.TryAdd(s.GetHashCode(), s);
                        }
                    }
                }

                output.Add(Symbol.End.GetHashCode(), Symbol.End);

                return output.Values;
            }
        }
        public IEnumerable<Symbol> NonTerminals 
        {
            get
            {
                var output = new Dictionary<int, Symbol>();

                foreach (Rule r in Rules)
                {
                    foreach (Symbol s in r.Left)
                    {
                        if (!s.IsTerminal)
                        {
                            output.TryAdd(s.GetHashCode(), s);
                        }
                    }
                    foreach (Symbol s in r.Right)
                    {
                        if (!s.IsTerminal)
                        {
                            output.TryAdd(s.GetHashCode(), s);
                        }
                    }
                }

                return output.Values;
            }
        }
    }
}

