namespace ABJAD.Models.Exceptions
{
    public class AbjadParsingException : AbjadException
    {
        public AbjadParsingException(string msg_en, string msg_ar)
            : base(msg_en, msg_ar)
        {
        }
    }
}
