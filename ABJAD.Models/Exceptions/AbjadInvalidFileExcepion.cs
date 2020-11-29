using static ABJAD.Models.Constants;

namespace ABJAD.Models.Exceptions
{
    public class AbjadInvalidFileExcepion : AbjadException
    {
        public AbjadInvalidFileExcepion() 
            : base(ErrorMessages.English.Extension, ErrorMessages.Arabic.Extension)
        {
        }
    }
}
