using EMP.Automata.FSM;
using System;
using System.Collections.Generic;

namespace EMP.Syntax.Lexing
{
    /// <summary>
    /// Represents tokenizer class utilizing deterministic finite automata in lexing process.
    /// </summary>
    /// <typeparam name="TEnum">Token type enumeration. DefaultTokenType enum can be used in case if no other specific TokenType enum is implemented.</typeparam>
    public class DFATokenizer<TEnum> : ITokenizer<TEnum> where TEnum : Enum
    {
        /// <summary>
        /// Internal instance of deterministic finite automata (DFA) class, used for input lexing.
        /// </summary>
        DFA<char, Token<TEnum>> _dfa;

        /// <summary>
        /// Returns instance of DFATokenizer class.
        /// </summary>
        /// <param name="dfa">Finite automata (DFA) class, used for input lexing</param>
        public DFATokenizer(DFA<char, Token<TEnum>> dfa)
        {
            _dfa = dfa;
        }

        /// <summary>
        /// Returns collection of tokens created from input string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IEnumerable<Token<TEnum>> Tokenize(string input)
        {
            return _dfa.Run(input);
        }
    }
}