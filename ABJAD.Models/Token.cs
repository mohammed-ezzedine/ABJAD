namespace ABJAD.Models
{
    public class Token
    {
        public Token(TokenType type, string text)
        {
            Type = type;
            Text = text;
        }

        public Token(TokenType type, char text)
            : this(type, text.ToString())
        {
        }

        public TokenType Type { get; set; }

        public string Text { get; set; }
    }
}
