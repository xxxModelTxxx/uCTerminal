using System;
using System.Text;

namespace EMP.Syntax.Grammar
{
    internal class Predict
    {

        private Rule rule;
        private Symbol symbol;

        public Predict(Rule r, Symbol s)
        {
            rule = r;
            symbol = s;
        }

        public Rule GetRule => rule;
        public Symbol GetSymbol => symbol;

        public override bool Equals(object obj)
        {
            Predict p = obj as Predict;

            if (p == null)
            {
                return false;
            }
            else
            {
                return p.GetRule.Equals(GetRule) && p.GetSymbol.Equals(GetSymbol);
            }
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(rule?.ToString(), symbol?.ToString());
        }
        public override string ToString()
        {
            var sb = new StringBuilder();

            return sb.Append('(').Append(rule.ToString()).Append(';').Append(symbol.ToString()).Append(')').ToString();
        }
    }
}
