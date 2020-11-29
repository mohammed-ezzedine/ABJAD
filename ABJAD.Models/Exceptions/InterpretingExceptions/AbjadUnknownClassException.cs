using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadUnknownClassException : AbjadInterpretingException
    {
        public AbjadUnknownClassException(string className, int line, int index) 
            : base(
                  ErrorMessages.English.UnknownClass(className, line, index),
                  ErrorMessages.Arabic.UnknownClass(className, line, index)
            )
        {
        }
    }
}
