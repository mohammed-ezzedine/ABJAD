namespace ABJAD.Models.Exceptions
{
    public class AbjadInvalidFileExcepion : AbjadExecption
    {
        public AbjadInvalidFileExcepion() : base("ABJAD files should have .abjad extention")
        {
        }
    }
}
