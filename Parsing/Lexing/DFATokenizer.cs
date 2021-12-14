using EMP.Automata.FiniteAutomata;
using System;
using System.Collections.Generic;

namespace EMP.Syntax.Lexing
{
    /// <summary>
    /// Represents tokenizer class utilizing deterministic finite state machine in lexing process.
    /// </summary>
    /// <typeparam name="TEnum">Token type enumeration. DefaultTokenType enum can be used in case if no other specific TokenType enum is implemented.</typeparam>
    public class DFATokenizer<TEnum> : ITokenizer<TEnum> where TEnum : Enum
    {
        /// <summary>
        /// Internal instance of deterministic finite state machine (DFSM) class, used for input lexing.
        /// </summary>
        private DFA<char, Token<TEnum>> _dfsm;

        /// <summary>
        /// Returns instance of DFSMTokenizer class.
        /// </summary>
        /// <param name="dfsm">Finite state machine (DFSM) class, used for input lexing</param>
        public DFATokenizer(DFA<char, Token<TEnum>> dfsm)
        {
            _dfsm = dfsm;
        }

        /// <summary>
        /// Returns collection of tokens created from input string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IEnumerable<Token<TEnum>> Tokenize(string input)
        {
            return _dfsm.Run(input);
        }
    }
}