using ABJAD.Models.Exceptions;
using System.IO;
using System.Linq;

namespace ABJAD.Reader
{
    public class AbjadFile
    {
        public static string Read(string fileName)
        {
            if (!IsAbjadFile(fileName))
            {
                throw new AbjadInvalidFileExcepion();
            }

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException();
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
