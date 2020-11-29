namespace ABJAD.Models
{
    public class Token
    {
        public Token(TokenType type, string text, int line, int index)
        {
            Type = type;
            Text = text;
            Line = line;
            Index = index;
        }

        public Token(TokenType type, char text, int line, int index)
            : this(type, text.ToString(), line, index)
        {
        }

        public TokenType Type { get; set; }

        public string Text { get; set; }

        public int Line { get; set; }

        public int Index { get; set; }
    }
}
