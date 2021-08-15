using System;

namespace EMP.Syntax.Grammar
{
    /// <summary>
    /// Represents InvalidRuleException class for Grammar class.
    /// </summary>
    [Serializable()]
    public class InvalidRuleException : Exception
    {
        /// <summary>
        /// Returns instance of InvalidRuleException class.
        /// </summary>
        /// <param name="rule">Rule that caused exception.</param>
        /// <param name="message">Additional message.</param>
        public InvalidRuleException(Rule rule, string message)
            : base(message)
        {
            ExRule = rule;
        }

        /// <summary>
        /// Returns rule which caused exception.
        /// </summary>
        public Rule ExRule { get; }
    }
}
