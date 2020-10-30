using System.IO;
using System.Linq;

namespace ABJAD.Reader
{
    public class Reader
    {
        public static string Read(string fileName)
        {
            if (!IsAbjadFile(fileName))
            {
                // TODO: throw new EZInvalidFileException();
            }

            if (!File.Exists(fileName))
            {
                // TODO: throw new EZFileNotFoundException(fileName);
            }

            var code = File.ReadAllText(fileName);
            return code;
        }

        private static bool IsAbjadFile(string fileName)
        {
            if (fileName.Split('.').Length < 2)
            {
                return false;
            }

            var extension = fileName.Split('.').Last().ToLower();
            if (extension == "abjad")
            {
                return true;
            }

            return false;
        }
    }
}
