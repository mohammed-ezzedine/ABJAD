namespace ABJAD.Exceptions
{
    public class AbjadUndefinedTokenException : AbjadLexingException
    {
        public AbjadUndefinedTokenException(int line, int index, string s) 
            : base($"Undefined token at line {line}:{index}: {s}")
        {
        }
    }
}
