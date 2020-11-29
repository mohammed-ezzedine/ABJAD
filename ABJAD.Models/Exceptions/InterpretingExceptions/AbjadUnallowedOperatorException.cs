using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions.InterpretingExceptions
{
    public class AbjadUnallowedOperatorException : AbjadInterpretingException
    {
        public AbjadUnallowedOperatorException(string oper, string type) 
            : base(
                  ErrorMessages.English.OperatorNotAllowed(oper, type),
                  ErrorMessages.Arabic.OperatorNotAllowed(oper, type)
            )
        {
        }
    }
}
