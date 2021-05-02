using System.Collections.Generic;
using System.Text;

namespace EMP.Syntax.Grammar
{
    public class Rule
    {
        private const string _definitionSymbol = "::=";

        public Rule(IEnumerable<Symbol> left, IEnumerable<Symbol> right)
        {
            Left = left;
            Right = right;
        }

        public IEnumerable<Symbol> Left { get; }
        public IEnumerable<Symbol> Right { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (Symbol s in Left)
            {
                sb.Append(s.Name).Append(' ');
            }

            sb.Append(_definitionSymbol);

            foreach(Symbol s in Right)
            {
                sb.Append(' ').Append(s.Name);
            }

            return sb.ToString();
        }
    }
}
