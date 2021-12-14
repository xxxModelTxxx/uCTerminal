using EMP.Syntax.Grammar;
using System;
using System.Collections.Generic;

namespace TestApp_Grammar
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFirstFollow();
        }

        #region Old functions

        static void TestFirstFollow()
        {
            //E → T X
            //X → +E
            //X → ε
            //T → int Y
            //T → (E)
            //Y → *T
            //Y → ε

            // var NS_S    = Symbol.Start;
            var NS_E = new Symbol("E");
            var NS_T = new Symbol("T");
            var NS_X = new Symbol("X");
            var NS_Y = new Symbol("Y");
            // var TS_e    = Symbol.End;
            var TS_Plus = new Symbol("+", true);
            var TS_Lpar = new Symbol("(", true);
            var TS_Rpar = new Symbol(")", true);
            var TS_Star = new Symbol("*", true);
            var TS_Int = new Symbol("int", true);

            var TestRules = new List<Rule>();
            // TestRules.Add(new Rule(new Symbol[] { NS_S }, new Symbol[] { NS_E, TS_e }));
            TestRules.Add(new Rule(new Symbol[] { NS_E }, new Symbol[] { NS_T, NS_X }));
            TestRules.Add(new Rule(new Symbol[] { NS_X }, new Symbol[] { TS_Plus, NS_E }));
            TestRules.Add(new Rule(new Symbol[] { NS_X }, new Symbol[] { Symbol.Epsilon }));
            TestRules.Add(new Rule(new Symbol[] { NS_T }, new Symbol[] { TS_Int, NS_Y }));
            TestRules.Add(new Rule(new Symbol[] { NS_T }, new Symbol[] { TS_Lpar, NS_E, TS_Rpar }));
            TestRules.Add(new Rule(new Symbol[] { NS_Y }, new Symbol[] { TS_Star, NS_T }));
            TestRules.Add(new Rule(new Symbol[] { NS_Y }, new Symbol[] { Symbol.Epsilon }));

            var TestGrammar = new LL1Grammar(TestRules);
            TestGrammar.PrintSummary();
        }
        #endregion
    }
}
