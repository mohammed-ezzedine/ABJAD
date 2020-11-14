using ABJAD.Models.Exceptions;
using ABJAD.Parser;
using System.Collections.Generic;

namespace ABJAD.Interpreter
{
    public class AbjadClass : IInstantiatable
    {
        public AbjadClass(Declaration.ClassDecl declaration, Environment envrionment)
        {
            Declaration = declaration;
            Environment = envrionment.Clone() as Environment;
        }

        public Declaration.ClassDecl Declaration { get; set; }

        public Environment Environment { get; set; }

        public object Instantiate(List<Expression> parameters)
        {
            Interpreter.AddClassFieldsAndFunctionsToScope(Declaration, Environment);
            var constructor = GetConstructor();
            //Interpreter.AddParamsToScope(constructor.Parameters, parameters, Environment);

            var initializer = new AbjadFunction(constructor, Environment);
            initializer.Call(parameters);

            //foreach (var param in Declaration.Parameters)
            //{
            //    var localInterpreter = new Interpreter(environment);
            //    var paramVal = param.Accept(localInterpreter);
            //    environment.Set(nameof(param), param);
            //}

            return new AbjadInstance(Environment, Declaration.Name);
        }

        public Declaration.FuncDecl GetConstructor()
        {
            foreach (var binding in ((Statement.BlockStmt)Declaration.Block).BindingList)
            {
                if (Interpreter.BindingIsFunct(binding, out var funcDecl) &&
                    funcDecl?.Name.Text == Declaration.Name.Text)
                {
                    return funcDecl;
                }
            }

            throw new AbjadInterpretingException($"No constructor for class {Declaration.Name.Text}.");
        }
    }
}
