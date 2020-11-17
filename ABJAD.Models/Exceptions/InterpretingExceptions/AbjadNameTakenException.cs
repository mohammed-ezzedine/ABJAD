namespace ABJAD.Models.Exceptions
{
    public class AbjadNameTakenException : AbjadInterpretingException
    {
        public AbjadNameTakenException(string name) 
            : base($"Name {name} already exists in the stack.")
        {
        }
    }
}
