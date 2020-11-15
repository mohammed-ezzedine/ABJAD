using ABJAD.Parser;
using System.Collections.Generic;

namespace ABJAD.InterpretEngine
{
    public interface IInstantiatable
    {
        public object Instantiate(List<Expression> paramaters);
    }
}