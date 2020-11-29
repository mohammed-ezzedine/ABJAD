using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadUnknownFunctionException : AbjadInterpretingException
    {
        public AbjadUnknownFunctionException(string funcName, int paramsCount) 
            : base(
                  ErrorMessages.English.UnknownFunction(funcName, paramsCount),
                  ErrorMessages.Arabic.UnknownFunction(funcName, paramsCount)
            )
        {
        }
    }
}
