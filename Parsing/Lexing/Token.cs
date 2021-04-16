using System;

namespace EMP.Parsing.Lexing
{
    // TODO: Consider changing Value identifier name to something else.
    public class Token<TEnum> where TEnum : Enum
    {
        public Token(TEnum tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public TEnum TokenType { get; }
        public string Value { get; }
    }
}
