namespace ABJAD.Models.Exceptions
{
    public class AbjadUnknownFunctionException : AbjadInterpretingException
    {
        public AbjadUnknownFunctionException(string funcName, int paramsCount) 
            : base($"There is no function called {funcName} that takes {paramsCount} parameters in the environment scope.")
        {
        }
    }
}
