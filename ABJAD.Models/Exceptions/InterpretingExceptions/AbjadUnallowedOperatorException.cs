namespace ABJAD.Models.Exceptions.InterpretingExceptions
{
    public class AbjadUnallowedOperatorException : AbjadInterpretingException
    {
        public AbjadUnallowedOperatorException(string oper, string type) 
            : base($"The {oper} operator is not allowed on data of type {type}.")
        {
        }
    }
}
