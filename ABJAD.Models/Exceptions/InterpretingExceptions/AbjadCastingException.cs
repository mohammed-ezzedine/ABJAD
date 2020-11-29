using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions.InterpretingExceptions
{
    public class AbjadCastingException : AbjadInterpretingException
    {
        public AbjadCastingException(string fromType, string toType)
            : base(
                  ErrorMessages.English.Casting(fromType, toType),
                  ErrorMessages.Arabic.Casting(fromType, toType)
            )
        {
        }

        public AbjadCastingException(string toType)
            : base(
                  ErrorMessages.English.Casting(toType),
                  ErrorMessages.Arabic.Casting(toType)
            )
        {
        }
    }
}
