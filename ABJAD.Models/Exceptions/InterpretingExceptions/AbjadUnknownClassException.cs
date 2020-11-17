namespace ABJAD.Models.Exceptions
{
    public class AbjadUnknownClassException : AbjadInterpretingException
    {
        public AbjadUnknownClassException(string className) 
            : base($"Class with name {className} doesn't exits in the environment scope.")
        {
        }
    }
}
