using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp_Strings
{
    internal class TokenDefinition
    {
        public TokenDefinition(TokenType tokenType, int priority, string regularExpression)
        {
            throw new System.NotImplementedException();
        }

        public TokenType TokenType { get ; }

        public int Priority { get ; }

        public string RegularExpression { get ; }
    }
}