using ABJAD.Parser;
using System.Collections.Generic;

namespace ABJAD.Interpreter
{
    public interface IInstantiatable
    {
        public object Instantiate(List<Expression> paramaters);
    }
}