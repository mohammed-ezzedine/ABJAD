using ABJAD.Models;
using System.Collections.Generic;

namespace ABJAD.Interpreter
{
    public class AbjadInstance
    {
        public AbjadInstance(Environment envrionment, Token type)
        {
            Environment = envrionment.Clone() as Environment;
            Type = type;
        }

        public Environment Environment { get; set; }

        public Token Type { get; set; }
    }
}
