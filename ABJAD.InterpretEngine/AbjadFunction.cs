using ABJAD.ParseEngine;
using System.Collections.Generic;

namespace ABJAD.InterpretEngine
{
    public class AbjadFunction : ICallable
    {
        private Environment environment;

        public AbjadFunction(Declaration.FuncDecl declaration, Environment environment)
        {
            Declaration = declaration;
            this.environment = environment.Clone() as Environment;
            this.environment.Set(declaration.Name.Text, this);
        }

        public Declaration.FuncDecl Declaration { get; set; }

        public object Call(List<object> parameters)
        {
            Interpreter.AddParamsToScope(Declaration.Parameters, parameters, environment);

            return Interpreter.ExecuteBlock(Declaration.Block, environment);
        }
    }
}
