using System;
using System.Text;

namespace ABJAD.IO
{
    public class BufferWriter : Writer
    {
        private StringBuilder buffer = new StringBuilder();

        public void Write(object txt)
        {
            buffer.AppendLine(txt.ToString());
        }

        public string GetOutput()
        {
            return buffer.ToString().TrimEnd();
        }
    }
}