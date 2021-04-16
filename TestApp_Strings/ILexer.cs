using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp_Strings
{
    public interface ILexer
    {
        List<Token> Tokenize(string text);
    }
}