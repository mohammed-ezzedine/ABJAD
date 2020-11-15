using ABJAD.Models;

namespace ABJAD.InterpretEngine
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
