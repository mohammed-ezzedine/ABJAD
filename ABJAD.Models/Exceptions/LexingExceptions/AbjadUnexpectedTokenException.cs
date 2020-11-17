namespace ABJAD.Models.Exceptions
{
    public class AbjadUnexpectedTokenException : AbjadLexingException
    {
        public AbjadUnexpectedTokenException(int line, int index) 
            : base($"Unexpected token at line {line}:{index}.")
        {
        }
    }
}
