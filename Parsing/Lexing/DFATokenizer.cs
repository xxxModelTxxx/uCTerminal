using System;
using System.Collections.Generic;

namespace EMP.Parsing.Lexing
{
    public class DFATokenizer<TEnum> : ITokenizer<TEnum> where TEnum : Enum
    {
        public DFATokenizer() { }

        public IEnumerable<Token<TEnum>> Tokenize(string input)
        {
            throw new NotImplementedException();
        }
    }
}