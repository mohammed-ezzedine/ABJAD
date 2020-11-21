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
            this.environment = environment;
            this.environment.Set(declaration.Name.Text, this);
        }

        public Declaration.FuncDecl Declaration { get; set; }

        public object Call(List<object> parameters)
        {
            var env = environment.Clone() as Environment;
            Interpreter.AddParamsToScope(Declaration.Parameters, parameters, env);

            return Interpreter.ExecuteBlock(Declaration.Block, env);
        }
    }
}
