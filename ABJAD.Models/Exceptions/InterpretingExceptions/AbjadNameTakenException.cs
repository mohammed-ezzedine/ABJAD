using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadNameTakenException : AbjadInterpretingException
    {
        public AbjadNameTakenException(string name) 
            : base(
                  ErrorMessages.English.NameTaken(name),
                  ErrorMessages.Arabic.NameTaken(name)
            )
        {
        }
    }
}
