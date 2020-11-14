using ABJAD.Parser;
using System.Collections.Generic;

namespace ABJAD.Interpreter
{
    public class AbjadFunction : ICallable
    {
        private Environment environment;

        public AbjadFunction(Declaration.FuncDecl declaration, Environment environment)
        {
            Declaration = declaration;
            this.environment = environment.Clone() as Environment;
        }

        public Declaration.FuncDecl Declaration { get; set; }

        public object Call(List<Expression> parameters)
        {
            Interpreter.AddParamsToScope(Declaration.Parameters, parameters, environment);

            //foreach (var param in Declaration.Parameters)
            //{
            //    var localInterpreter = new Interpreter(environment);
            //    var paramVal = param.Accept(localInterpreter);
            //    environment.Set(nameof(param), param);
            //}

            return Interpreter.ExecuteBlock(Declaration.Block, environment);
        }
    }
}
