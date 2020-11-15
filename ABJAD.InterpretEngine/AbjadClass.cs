using ABJAD.Models.Exceptions;
using ABJAD.ParseEngine;
using System.Collections.Generic;

namespace ABJAD.InterpretEngine
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

            var initializer = new AbjadFunction(constructor, Environment);
            initializer.Call(parameters);

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
