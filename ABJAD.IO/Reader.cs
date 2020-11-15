using ABJAD.Models.Exceptions;
using System.IO;
using System.Linq;

namespace ABJAD.IO
{
    public class Reader
    {
        public static string Read(string fileName)
        {
            if (!IsAbjadFile(fileName))
            {
                throw new AbjadInvalidFileExcepion();
            }

            if (!System.IO.File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }

            var code = System.IO.File.ReadAllText(fileName);
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
