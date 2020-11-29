using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions.InterpretingExceptions
{
    public class AbjadCastingException : AbjadInterpretingException
    {
        public AbjadCastingException(string fromType, string toType, int line, int index)
            : base(
                  ErrorMessages.English.Casting(fromType, toType, line, index),
                  ErrorMessages.Arabic.Casting(fromType, toType, line, index)
            )
        {
        }

        public AbjadCastingException(string toType, int line, int index)
            : base(
                  ErrorMessages.English.Casting(toType, line, index),
                  ErrorMessages.Arabic.Casting(toType, line, index)
            )
        {
        }
    }
}
