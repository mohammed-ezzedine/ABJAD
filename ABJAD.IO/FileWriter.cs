using System.Collections.Generic;
using System.Linq;

namespace ABJAD.IO
{
    public class FileWriter : Writer
    {
        private readonly string fileName;
        private readonly List<string> output;
        private readonly bool online;

        public FileWriter(string fileName, List<string> output = null, bool online = false)
        {
            this.online = online;
            this.fileName = fileName;
            if (online)
            {
                this.output = output?? new List<string>();
            }
            else
            {
                System.IO.File.WriteAllText(fileName, "");
            }
        }

        public void Write(object txt)
        {
            if (online)
            {
                output.Add(txt.ToString());
            }
            else
            {
                System.IO.File.AppendAllText(fileName, txt.ToString() + "\n");
            }
        }

        public string GetOutput()
        {
            return output.Count > 0? output.Aggregate((prev, next) => prev + "\n" + next) : "";
        }
    }
}
