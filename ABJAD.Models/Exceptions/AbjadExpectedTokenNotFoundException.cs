namespace ABJAD.Models.Exceptions
{
    public class AbjadExpectedTokenNotFoundException : AbjadLexingException
    {
        public AbjadExpectedTokenNotFoundException(int line, string token) 
            : base($"Expected token: '{token}' was not found at line: {line}")
        {
        }
    }
}
