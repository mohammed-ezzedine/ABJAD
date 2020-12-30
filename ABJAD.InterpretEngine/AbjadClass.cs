using ABJAD.Models.Exceptions;
using ABJAD.ParseEngine;
using System.Collections.Generic;
using static ABJAD.Models.Constants;

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

        public object Instantiate(List<object> parameters)
        {
            var instanceEnvironment = Environment.Clone() as Environment;

            Interpreter.AddClassFieldsAndFunctionsToScope(Declaration, instanceEnvironment);
            var constructor = GetConstructor(parameters.Count);

            var initializer = new AbjadFunction(constructor, instanceEnvironment);
            initializer.Call(parameters);

            return new AbjadInstance(instanceEnvironment, Declaration.Name);
        }

        public Declaration.FuncDecl GetConstructor(int parametersCount)
        {
            foreach (var binding in ((Statement.BlockStmt)Declaration.Block).BindingList)
            {
                if (Interpreter.BindingIsFunct(binding, out var funcDecl) 
                    && funcDecl?.Name.Text == Declaration.Name.Text
                    && funcDecl?.Parameters.Count == parametersCount)
                {
                    return funcDecl;
                }
            }

            throw new AbjadInterpretingException(
                ErrorMessages.English.UnfoundConstructor(Declaration.Name.Text, parametersCount),
                ErrorMessages.Arabic.UnfoundConstructor(Declaration.Name.Text, parametersCount)
            );
        }
    }
}
