using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadUnknownClassException : AbjadInterpretingException
    {
        public AbjadUnknownClassException(string className) 
            : base(
                  ErrorMessages.English.UnknownClass(className),
                  ErrorMessages.Arabic.UnknownClass(className)
            )
        {
        }
    }
}
