using System;

namespace ABJAD.Models.Exceptions
{
    public class AbjadExecption : Exception
    {
        public AbjadExecption(string msg)
            : base(msg)
        {
        }
    }
}
