namespace ABJAD.Models
{
    public enum TokenType
    {
        /* Operations */
        AND, BANG, BANG_EQUAL, DIVIDED_BY, EQUAL, EQUAL_EQUAL, GREATER_EQUAL,
        GREATER_THAN, LESS_EQUAL, LESS_THAN, MINUS, OR, PLUS, TIMES, 

        /* Other graphic characters */
        CLOSE_BRACE, CLOSE_PAREN, COMMA, DOT, OPEN_BRACE, OPEN_PAREN, SEMICOLON,

        /* Keywords */
        BOOL, CLASS, CONST, ELSE, FALSE, FOR, FUNC, IF, NEW, NULL, NUMBER, PRINT, RETURN, STRING, TYPEOF, TRUE, WHILE, VAR,

        /* Primitives */
        NUMBER_CONST, ID, STRING_CONST
    }
}
