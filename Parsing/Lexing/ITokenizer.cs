using System;
using System.Collections.Generic;

namespace EMP.Parsing.Lexing
{
    public interface ITokenizer<TEnum> where TEnum : Enum
    {
        public IEnumerable<Token<TEnum>> Tokenize(string input);
    }
}
