using ABJAD.Parser;
using System.Collections.Generic;

namespace ABJAD.Interpreter
{
    public interface ICallable
    {
        public object Call(List<Expression> paramters);
    }
}
