using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadNameTakenException : AbjadInterpretingException
    {
        public AbjadNameTakenException(string name, int line, int index) 
            : base(
                  ErrorMessages.English.NameTaken(name, line, index),
                  ErrorMessages.Arabic.NameTaken(name, line, index)
            )
        {
        }
    }
}
