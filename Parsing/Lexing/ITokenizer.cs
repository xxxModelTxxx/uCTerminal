using System;
using System.Collections.Generic;

namespace EMP.Syntax.Lexing
{
    public interface ITokenizer<TEnum> where TEnum : Enum
    {
        public IEnumerable<Token<TEnum>> Tokenize(string input);
    }
}
