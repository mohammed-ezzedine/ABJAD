using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadUnknownFunctionException : AbjadInterpretingException
    {
        public AbjadUnknownFunctionException(string funcName, int paramsCount, int line, int index) 
            : base(
                  ErrorMessages.English.UnknownFunction(funcName, paramsCount, line, index),
                  ErrorMessages.Arabic.UnknownFunction(funcName, paramsCount, line, index)
            )
        {
        }
    }
}
