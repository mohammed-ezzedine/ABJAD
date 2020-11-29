using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadUndefinedTokenException : AbjadLexingException
    {
        public AbjadUndefinedTokenException(int line, int index, string s) 
            : base(
                  ErrorMessages.English.UndefinedToken(line, index, s),
                  ErrorMessages.Arabic.UndefinedToken(line, index, s)
            )
        {
        }
    }
}
