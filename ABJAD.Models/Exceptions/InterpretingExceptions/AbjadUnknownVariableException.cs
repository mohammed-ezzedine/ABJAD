using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadUnknownVariableException : AbjadInterpretingException
    {
        public AbjadUnknownVariableException(string varName)
            : base(
                  ErrorMessages.English.UnknownVariable(varName),
                  ErrorMessages.Arabic.UnknownVariable(varName)
            )
        {
        }
    }
}
