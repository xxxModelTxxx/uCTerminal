using EMP.Syntax.Grammar;
using System;
using System.Collections.Generic;

namespace TestApp_Grammar
{
    class Program
    {
        static void Main(string[] args)
        {
            //< start >         ::=     < syntax > #EOS
            //< syntax >        ::=     < identifier > < parameters >
            //< identifier >    ::=     id dot<identifier>
            //< identifier >    ::=     id
            //< parameters >    ::=     lpar < attribute > rpar
            //< attribute >     ::=     < type > comma<attribute>
            //< attribute >     ::=     < type >
            //< type >          ::=     type


            /*
            // Non-Terminal
            var NS_Syntax       = new Symbol("<syntax>");
            var NS_Identifier   = new Symbol("<identifier>");
            var NS_Parameters   = new Symbol("<parameters>");
            var NS_Attribute    = new Symbol("<symbol>");
            var NS_Type         = new Symbol("<type>");  
            // Terminals
            var TS_Id           = new Symbol("id", true);
            var TS_Dot          = new Symbol("dot", true);
            var TS_Lpar         = new Symbol("lpar", true);
            var TS_Rpar         = new Symbol("rpar", true);
            var TS_Comma        = new Symbol("comma", true);
            var TS_Type         = new Symbol("type", true);

            // Production rules
            var Rules = new List<Rule>();
            Rules.Add(new Rule(new Symbol[] { Symbol.Start }, new Symbol[] { NS_Syntax, Symbol.End }));
            Rules.Add(new Rule(new Symbol[] { NS_Syntax }, new Symbol[] { NS_Identifier, NS_Parameters }));
            Rules.Add(new Rule(new Symbol[] { NS_Identifier }, new Symbol[] { TS_Id, TS_Dot, NS_Identifier }));
            Rules.Add(new Rule(new Symbol[] { NS_Identifier }, new Symbol[] { TS_Id }));
            Rules.Add(new Rule(new Symbol[] { NS_Attribute }, new Symbol[] { NS_Type, TS_Comma, NS_Attribute }));
            Rules.Add(new Rule(new Symbol[] { NS_Attribute }, new Symbol[] { NS_Type }));
            Rules.Add(new Rule(new Symbol[] { NS_Type }, new Symbol[] { TS_Type }));
            */

            //E → T X
            //X → +E
            //X → ε
            //T → int Y
            //T → (E)
            //Y → *T
            //Y → ε

            var NS_E    = Symbol.Start;
            var NS_T    = new Symbol("T");
            var NS_X    = new Symbol("X");
            var NS_Y    = new Symbol("Y");
            var TS_Plus = new Symbol("+", true);
            var TS_Lpar = new Symbol("(", true);
            var TS_Rpar = new Symbol(")", true);
            var TS_Star = new Symbol("*", true);
            var TS_Int  = new Symbol("int", true);

            var TestRules = new List<Rule>();
            TestRules.Add(new Rule(new Symbol[] { NS_E }, new Symbol[] { NS_T, NS_X }));
            TestRules.Add(new Rule(new Symbol[] { NS_X }, new Symbol[] { TS_Plus, NS_E }));
            TestRules.Add(new Rule(new Symbol[] { NS_X }, new Symbol[] { Symbol.Epsilon }));
            TestRules.Add(new Rule(new Symbol[] { NS_T }, new Symbol[] { TS_Int, NS_Y }));
            TestRules.Add(new Rule(new Symbol[] { NS_T }, new Symbol[] { TS_Lpar, NS_E, TS_Rpar }));
            TestRules.Add(new Rule(new Symbol[] { NS_Y }, new Symbol[] { TS_Star, NS_T }));
            TestRules.Add(new Rule(new Symbol[] { NS_Y }, new Symbol[] { Symbol.Epsilon }));

            var TestGrammar = new LL1Grammar(TestRules);

            Console.WriteLine("{0,-20} {1,-20}", NS_E.Name, TestGrammar.IsNullabe(NS_E));
            Console.WriteLine("{0,-20} {1,-20}", NS_T.Name, TestGrammar.IsNullabe(NS_T));
            Console.WriteLine("{0,-20} {1,-20}", NS_X.Name, TestGrammar.IsNullabe(NS_X));
            Console.WriteLine("{0,-20} {1,-20}", NS_Y.Name, TestGrammar.IsNullabe(NS_Y));
            Console.ReadLine();
        }
    }
}
