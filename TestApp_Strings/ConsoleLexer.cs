using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TestApp_Strings
{
    public class ConsoleLexer : ILexer
    {
        private List<TokenDefinition> _tokenDefinitions;
        private List<Match> _tokenMatches;

        public List<Token> Tokenize(string text)
        {
            throw new NotImplementedException();
        }
    }
}