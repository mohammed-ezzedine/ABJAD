using ABJAD.IO;
using ABJAD.Models.Exceptions;
using ABJAD.ParseEngine;
using System.Collections.Generic;
using System.Linq;
using static ABJAD.Models.TokenType;

namespace ABJAD.InterpretEngine
{
    public class Interpreter : 
        Binding.IVisitor<object>, 
        Declaration.IVisitor<object>, 
        Expression.IVisitor<object>, 
        Primitive.IVisitor<object>, 
        Statement.Visitor<object>
    {
        private Environment environment;
        private bool _return = false;
        private object _returned;

        public Interpreter(Writer writer, Environment environment, bool referenceScope = false)
        {
            this.environment = referenceScope ? environment : environment.Clone() as Environment;
            Writer = writer;
        }

        public Interpreter(Writer writer)
        {
            environment = new Environment();
            Writer = writer;
        }

        public static Writer Writer { get; set; }

        public void Interpret(List<Binding> bindings)
        {
            foreach (var binding in bindings)
            {
                Interpret(binding);
            }
        }

        private void Interpret(Binding binding)
        {
            binding.Accept(this);
        }

        private object Evaluate(Expression expr)
        {
            return expr.Accept(this);
        }

        public object VisitDeclBinding(Binding.DeclBinding binding)
        {
            return binding.Decl.Accept(this);
        }

        public object VisitStmtBinding(Binding.StmtBinding binding)
        {
            return binding.Stmt.Accept(this);
        }

        public object VisitFuncDecl(Declaration.FuncDecl declaration)
        {
            if (environment.ContainsKey(declaration.Name.Text))
            {
                throw new AbjadNameTakenException(declaration.Name.Text);
            }

            var abjadFunction = new AbjadFunction(declaration, environment.Clone() as Environment);
            environment.Set(declaration.Name.Text, abjadFunction);
            return null;
        }

        public object VisitConstDecl(Declaration.ConstDecl declaration)
        {
            if (environment.ContainsKey(declaration.Name.Text))
            {
                throw new AbjadNameTakenException(declaration.Name.Text);
            }

            environment.SetConstant(declaration.Name.Text, Evaluate(declaration.Value));
            return null;
        }

        public object VisitVarDecl(Declaration.VarDecl declaration)
        {
            if (environment.ContainsKey(declaration.Name.Text))
            {
                throw new AbjadNameTakenException(declaration.Name.Text);
            }

            environment.Set(declaration.Name.Text, Evaluate(declaration.Value)); 
            return null;
        }

        public object VisitClassDecl(Declaration.ClassDecl declaration)
        {
            if (environment.ContainsKey(declaration.Name.Text))
            {
                throw new AbjadNameTakenException(declaration.Name.Text);
            }

            var abjadClass = new AbjadClass(declaration, environment);
            environment.Set(declaration.Name.Text, abjadClass);

            return null;
        }

        public object VisitCallExpr(Expression.CallExpr expr)
        {
            AbjadFunction abjadFunc = null;

            if (expr.Class == null)
            {
                abjadFunc = environment.Get(expr.Func.Text) as AbjadFunction;
            }
            else
            {
                var abjadInstance = environment.Get(expr.Class.Text) as AbjadInstance;
                if (abjadInstance == null)
                {
                    throw new AbjadUnknownClassException(expr.Class.Text);
                }

                var abjadClass = environment.Get(abjadInstance.Type.Text) as AbjadClass;
                var abjadClassBlock = abjadClass.Declaration.Block as Statement.BlockStmt;

                foreach (var binding in abjadClassBlock.BindingList)
                {
                    if (BindingIsFunct(binding, out var funcDecl) &&
                        funcDecl.Parameters.Count == expr.Parameters.Count)
                    {
                        abjadFunc = new AbjadFunction(funcDecl, abjadClass.Environment);
                    }
                }
            }

            if (abjadFunc == null ||
                abjadFunc.Declaration.Parameters.Count != expr.Parameters.Count)
            {
                throw new AbjadUnknownFunctionException(expr.Func.Text, expr.Parameters.Count);
            }

            var parameters = expr.Parameters.Select(p => Evaluate(p)).ToList();
            
            return abjadFunc.Call(parameters);
        }

        public object VisitFieldExpr(Expression.FieldExpr expr)
        {
            var @class = environment.Get(expr.Class.Text) as AbjadInstance;
            if (@class == null)
            {
                throw new AbjadUnknownClassException(expr.Class.Text);
            }

            return @class.Environment.Get(expr.Field.Text);
        }

        public object VisitInstExpr(Expression.InstExpr expr)
        {
            var @class = environment.Get(expr.Class.Text) as AbjadClass;
            if (@class == null)
            {
                throw new AbjadUnknownClassException(expr.Class.Text);
            }

            var classBlock = @class.Declaration.Block as Statement.BlockStmt;
            if (classBlock == null)
            {
                throw new AbjadInterpretingException("Invalid class block syntax.");
            }

            var parameters = expr.Parameters.Select(p => Evaluate(p)).ToList();

            return @class.Instantiate(parameters);
        }

        public object VisitUnaryExpr(Expression.UnaryExpr expr)
        {
            var operand = Evaluate(expr.Operand) as AbjadObject;
            switch (expr.Operation.Type)
            {
                case MINUS:
                    return operand.OperatorUMinus();
                case BANG:
                    return operand.OperatorBang();
                default:
                    throw new AbjadInterpretingException("Invalid unary expression.");
            }
        }

        public object VisitBinaryExpr(Expression.BinaryExpr expr)
        {
            var oper1 = Evaluate(expr.Operand1) as AbjadObject;
            var oper2 = Evaluate(expr.Operand2) as AbjadObject;

            switch (expr.Operation.Type)
            {
                case AND:
                    return oper1.OperatorAnd(oper2);
                case OR:
                    return oper1.OperatorOr(oper2);
                case BANG_EQUAL:
                    return new AbjadBool(!(bool)oper1.OperatorEqual(oper2).Value);
                case EQUAL_EQUAL:
                    return oper1.OperatorEqual(oper2);
                case DIVIDED_BY:
                    return oper1.OperatorDiviededBy(oper2);
                case GREATER_EQUAL:
                    return oper1.OperatorGreaterEqual(oper2);
                case GREATER_THAN:
                    return oper1.OperatorGreaterThan(oper2);
                case LESS_EQUAL:
                    return oper1.OperatorLessEqual(oper2);
                case LESS_THAN:
                    return oper1.OperatorLessThan(oper2);
                case MINUS:
                    return oper1.OperatorMinus(oper2);
                case PLUS:
                    return oper1.OperatorPlus(oper2);
                case TIMES:
                    return oper1.OperatorTimes(oper2);
                default:
                    throw new AbjadInterpretingException("Invalid binary expression.");
            }
        }

        public object VisitGroupExpr(Expression.GroupExpr expr)
        {
            return Evaluate(expr.Expr);
        }

        public object VisitPrimitiveExpr(Expression.PrimitiveExpr expr)
        {
            return expr.Primitive.Accept(this);
        }

        public object VisitNumberConst(Primitive.NumberConst numberConst) => new AbjadNumber(numberConst.value);

        public object VisitStringConst(Primitive.StringConst stringConst) => new AbjadString(stringConst.value);

        public object VisitTrue(Primitive.True @true) => new AbjadBool(true);

        public object VisitFalse(Primitive.False @false) => new AbjadBool(false);

        public object VisitNull(Primitive.Null @null) => null;

        public object VisitIdentifier(Primitive.Identifier identifier) =>  environment.Get(identifier.value);

        public object VisitExprStmt(Statement.ExprStmt stmt)
        {
            Evaluate(stmt.Expr);
            return null;
        }

        public object VisitIfStmt(Statement.IfStmt stmt)
        {
            var conditionObj = Evaluate(stmt.Condition) as AbjadBool;
            if ((bool)conditionObj.Value)
            {
                return ExecuteBlock(stmt.IfBlock, environment);         // TODO: check whether the return is necessary
            }
            else if(stmt.ElseBlock != null)
            {
                return ExecuteBlock(stmt.ElseBlock, environment);
            }

            return null;
        }

        public object VisitWhileStmt(Statement.WhileStmt stmt)
        {
            var conditionObj = Evaluate(stmt.Condition) as AbjadBool;
            while ((bool)conditionObj.Value)
            {
                ExecuteBlock(stmt.Block, environment);
                conditionObj = Evaluate(stmt.Condition) as AbjadBool;
            }

            return null;
        }

        public object VisitForStmt(Statement.ForStmt stmt)
        {
            stmt.Declaration.Accept(this);
            var conditionObj = Evaluate(stmt.Condition) as AbjadBool;
            while ((bool)conditionObj.Value)
            {
                ExecuteBlock(stmt.Block, environment);
                stmt.Assignment.Accept(this);
                conditionObj = Evaluate(stmt.Condition) as AbjadBool;
            }

            return null;
        }

        public object VisitBlockStmt(Statement.BlockStmt stmt) => ExecuteBlock(stmt, environment);

        public object VisitReturnStmt(Statement.ReturnStmt stmt)        // TODO: get rid of the unneeded global variables
        {
            _returned = Evaluate(stmt.Expr);
            _return = true;
            environment._return = true;
            environment._returned = _returned;
            return new AbjadReturn(_returned);
        }

        public object VisitAssignmentStmt(Statement.AssignmentStmt stmt)
        {
            var variable = environment.Get(stmt.Variable.Text);

            environment.Set(stmt.Variable.Text, Evaluate(stmt.Value));
            return null;
        }

        public object VisitPrintStmt(Statement.PrintStmt stmt)
        {
            var val = Evaluate(stmt.Expr) as AbjadObject;
            var edited = val == null? "عدم" : val.ToStr().Value as string;
            Writer.Write(edited);
            return null;
        }

        public static object ExecuteBlock(Statement.BlockStmt block, Environment env)
        {
            env._return = false;
            env._returned = null;
            //env.Set("_return", false);
            //env.Set("_returned", null);

            var localInterpret = new Interpreter(Writer, env);
            localInterpret._return = false;
            localInterpret._returned = null;
            foreach (var binding in block.BindingList)
            {
                var result = binding.Accept(localInterpret);

                if (result is AbjadReturn)
                {
                    return result;
                }
            }

            return null;
        }

        public static void AddParamsToScope(List<Expression> parametersDef, List<object> parameters, Environment scope)
        {
            for (int i = 0; i < parametersDef.Count; i++)
            {
                // We want to get the parameter name from the funcion decaration to make it our key
                // for the value evaluated from the expression passed in the function call.

                var defPrimtiveExpr = parametersDef[i] as Expression.PrimitiveExpr;
                var defPrimitive = defPrimtiveExpr?.Primitive as Primitive.Identifier;
                var paramName = defPrimitive?.value;

                var paramValue = parameters[i];
                scope.Set(paramName, paramValue);
            }
        }

        public static void AddClassFieldsAndFunctionsToScope(Declaration.ClassDecl @class, Environment scope)
        {
            var localInterpreter = new Interpreter(Writer, scope, true);
            foreach (var binding in ((Statement.BlockStmt)@class.Block).BindingList)
            {
                if (!BindingIsFunct(binding, out var funcDecl) ||
                    @class.Name.Text != funcDecl.Name.Text) // don't add the constructor to the stack, because it has the same name as the class
                {
                    localInterpreter.Interpret(binding);
                }
            }
        }

        public static bool BindingIsFunct(Binding binding, out Declaration.FuncDecl funcDecl)
        {
            var decl = binding as Binding.DeclBinding;
            if (decl != null)
            {
                funcDecl = decl.Decl as Declaration.FuncDecl;
                if (funcDecl != null)
                {
                    return true;
                }
            }

            funcDecl = null;
            return false;
        }

        private static bool bindingIsReturn(Binding binding)
        {
            var stmt = binding as Binding.StmtBinding;
            if (stmt != null)
            {
                var returnStmt = stmt.Stmt as Statement.ReturnStmt;
                if (returnStmt != null) return true;
            }

            return false;
        }
    }
}
