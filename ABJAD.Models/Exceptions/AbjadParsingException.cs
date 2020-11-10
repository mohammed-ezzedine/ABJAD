using System;

namespace ABJAD.Models.Exceptions
{
    public class AbjadParsingException : Exception
    {
        public AbjadParsingException(string msg)
            : base(msg)
        {
        }
    }
}
