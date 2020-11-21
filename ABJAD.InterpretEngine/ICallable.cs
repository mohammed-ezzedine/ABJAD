using System.Collections.Generic;

namespace ABJAD.InterpretEngine
{
    public interface ICallable
    {
        public object Call(List<object> parameters);
    }
}
