namespace ABJAD.Models
{
    public enum TokenType
    {
        /* Operations */
        AND, BANG, BANG_EQUAL, DIVIDED_BY, EQUAL, EQUAL_EQUAL, GREATER_EQUAL,
        GREATER_THAN, LESS_EQUAL, LESS_THAN, MINUS, OR, PLUS, TIMES, 

        /* Other graphic characters */
        CLOSE_BRACE, CLOSE_PAREN, COMMA, DOT, DOUBLE_SLASH, OPEN_BRACE, OPEN_PAREN, SEMICOLON,

        /* Keywords */
        CLASS, CONST, ELSE, FALSE, FOR, FUNC, IF, NEW, NULL, PRINT, RETURN, TRUE, WHILE, VAR,

        /* Primitives */
        NUMBER_CONST, ID, STRING_CONST
    }
}
