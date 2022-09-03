namespace ABJAD.IO
{
    public interface Writer
    {
        void Write(object txt);
        string GetOutput();
    }
}