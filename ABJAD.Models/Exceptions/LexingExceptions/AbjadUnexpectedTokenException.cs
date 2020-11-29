using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadUnexpectedTokenException : AbjadLexingException
    {
        public AbjadUnexpectedTokenException(int line, int index, string token) 
            : base(
                  ErrorMessages.English.UnexpectedToken(line, index, token),
                  ErrorMessages.Arabic.UnexpectedToken(line, index, token)
                )
        {
        }
    }
}
