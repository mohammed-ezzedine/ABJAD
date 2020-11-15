namespace ABJAD.IO
{
    public class Writer
    {
        private readonly string fileName;

        public Writer(string fileName)
        {
            this.fileName = fileName;
        }

        public void Write(object txt)
        {
            System.IO.File.WriteAllText(fileName, txt.ToString());
        }
    }
}
