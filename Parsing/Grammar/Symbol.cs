namespace EMP.Syntax.Grammar
{
    public class Symbol
    {
        public static readonly Symbol End = new Symbol("#END", true);
        public static readonly Symbol Epsilon = new Symbol("#EPSILON", true);
        public static readonly Symbol Error = new Symbol("#ERROR", true);
        public static readonly Symbol Start = new Symbol("#START", false);

        public Symbol(string name, bool isTerminal = false)
        {
            IsTerminal = isTerminal;
            Name = name;
        }

        public bool IsTerminal { get; }
        public string Name { get; }

        public override bool Equals(object obj)
        {
            Symbol s = obj as Symbol;

            if (s == null)
            {
                return false;
            }
            else
            {
                return s.Name == Name && s.IsTerminal == IsTerminal;
            }
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
