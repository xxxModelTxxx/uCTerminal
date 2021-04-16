namespace EMP.Parsing.Lexing
{
    // TODO: Fill enum with remaining tokens
    public enum DefaultTokenType
    {
        // ---
        StartOfText,
        EndOfText,
        Error,
        // ---
        Keyword,
        Identifier,
        // ---
        Space,
        NewLine,
        // ---
        Dot,
        Comma,
        Semicolon,
        Backslash,
        LeftBracket,
        RightBracket,
        // ---
        Character,
        String,
        Bool,
        Byte,
        UnsignedInt,
        Int,
        UnsignedLong,
        Long,
        Float,
        Double,
    }
}