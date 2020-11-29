using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadExpectedTokenNotFoundException : AbjadLexingException
    {
        public AbjadExpectedTokenNotFoundException(string token, int line, int index) 
            : base(
                  ErrorMessages.English.ExpectedToken(token, line, index),
                  ErrorMessages.Arabic.ExpectedToken(token, line, index)
            )
        {
        }
    }
}
