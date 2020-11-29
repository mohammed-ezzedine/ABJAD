using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions.InterpretingExceptions
{
    public class AbjadUnallowedOperatorException : AbjadInterpretingException
    {
        public AbjadUnallowedOperatorException(string oper, string type, int line, int index) 
            : base(
                  ErrorMessages.English.OperatorNotAllowed(oper, type, line, index),
                  ErrorMessages.Arabic.OperatorNotAllowed(oper, type, line, index)
            )
        {
        }
    }
}
