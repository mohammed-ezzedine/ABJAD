using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadExpectedTokenNotFoundException : AbjadLexingException
    {
        public AbjadExpectedTokenNotFoundException(int line, string token) 
            : base(
                  ErrorMessages.English.ExpectedToken(line, token),
                  ErrorMessages.Arabic.ExpectedToken(line, token)
            )
        {
        }
    }
}
