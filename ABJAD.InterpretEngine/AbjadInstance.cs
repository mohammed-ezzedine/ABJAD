using ABJAD.Models;

namespace ABJAD.InterpretEngine
{
    public class AbjadInstance : AbjadObject
    {
        public AbjadInstance(Environment envrionment, Token type)
        {
            Environment = envrionment.Clone() as Environment;
            Type = type;
            Value = this;
        }

        public Environment Environment { get; set; }

        public new Token Type { get; set; }

        public override AbjadString GetType()
        {
            return new AbjadString(Type.Text);
        }
    }
}
