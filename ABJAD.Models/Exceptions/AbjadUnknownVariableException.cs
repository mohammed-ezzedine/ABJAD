namespace ABJAD.Models.Exceptions
{
    public class AbjadUnknownVariableException : AbjadInterpretingException
    {
        public AbjadUnknownVariableException(string varName)
            : base($"Variable of name {varName} is undefined.")
        {
        }
    }
}
