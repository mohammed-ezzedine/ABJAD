namespace ABJAD.Models.Exceptions.InterpretingExceptions
{
    public class AbjadCastingException : AbjadInterpretingException
    {
        public AbjadCastingException(string fromType, string toType)
            : base($"Cannot cast from type {fromType} to type {toType}.")
        {
        }
    }
}
