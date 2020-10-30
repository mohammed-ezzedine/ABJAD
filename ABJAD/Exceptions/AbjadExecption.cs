using System;

namespace ABJAD.Exceptions
{
    public class AbjadExecption : Exception
    {
        public AbjadExecption(string msg)
            : base(msg)
        {
        }
    }
}
