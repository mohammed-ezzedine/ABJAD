using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadUnknownVariableException : AbjadInterpretingException
    {
        public AbjadUnknownVariableException(string varName, int line, int index)
            : base(
                  ErrorMessages.English.UnknownVariable(varName, line, index),
                  ErrorMessages.Arabic.UnknownVariable(varName, line, index)
            )
        {
        }
    }
}
