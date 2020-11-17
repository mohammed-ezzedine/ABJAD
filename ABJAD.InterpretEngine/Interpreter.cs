using ABJAD.IO;
using ABJAD.Models.Exceptions;
using ABJAD.ParseEngine;
using System.Collections.Generic;
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

            var abjadFunction = new AbjadFunction(declaration, environment);
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
            
            return abjadFunc.Call(expr.Parameters);
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

            return @class.Instantiate(expr.Parameters);
        }

        public object VisitUnaryExpr(Expression.UnaryExpr expr)
        {
            var operand = Evaluate(expr.Operand);
            switch (expr.Operation.Type)
            {
                case MINUS:
                    return -1 * (double)operand;
                case BANG:
                    return !(bool)operand;
                default:
                    throw new AbjadInterpretingException("Invalid unary expression.");
            }
        }

        public object VisitBinaryExpr(Expression.BinaryExpr expr)
        {
            var oper1 = Evaluate(expr.Operand1);
            var oper2 = Evaluate(expr.Operand2);

            switch (expr.Operation.Type)
            {
                case AND:
                    return (bool)oper1 && (bool)oper2;
                case OR:
                    return (bool)oper1 || (bool)oper2;
                case BANG_EQUAL:
                    return !oper1.Equals(oper2);
                case EQUAL_EQUAL:
                    return oper1.Equals(oper2);
                case DIVIDED_BY:
                    return (double)oper1 / (double)oper2;
                case GREATER_EQUAL:
                    return (double)oper1 >= (double)oper2;
                case GREATER_THAN:
                    return (double)oper1 > (double)oper2;
                case LESS_EQUAL:
                    return (double)oper1 <= (double)oper2;
                case LESS_THAN:
                    return (double)oper1 < (double)oper2;
                case MINUS:
                    return (double)oper1 - (double)oper2;
                case PLUS:
                    return (double)oper1 + (double)oper2;
                case TIMES:
                    return (double)oper1 * (double)oper2;
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

        public object VisitNumberConst(Primitive.NumberConst numberConst) => numberConst.value;

        public object VisitStringConst(Primitive.StringConst stringConst) => stringConst.value;

        public object VisitTrue(Primitive.True @true) => true;

        public object VisitFalse(Primitive.False @false) => false;

        public object VisitNull(Primitive.Null @null) => null;

        public object VisitIdentifier(Primitive.Identifier identifier)
        {
            if (environment.ContainsKey(identifier.value))
            {
                return environment.Get(identifier.value);
            }

            return identifier.value;
        }

        public object VisitExprStmt(Statement.ExprStmt stmt)
        {
            return Evaluate(stmt.Expr);
        }

        public object VisitIfStmt(Statement.IfStmt stmt)
        {
            var condition = (bool)Evaluate(stmt.Condition);
            if (condition)
            {
                ExecuteBlock(stmt.IfBlock, environment);
            }
            else if(stmt.ElseBlock != null)
            {
                ExecuteBlock(stmt.ElseBlock, environment);
            }

            return null;
        }

        public object VisitWhileStmt(Statement.WhileStmt stmt)
        {
            var condition = (bool)Evaluate(stmt.Condition);
            while (condition)
            {
                ExecuteBlock(stmt.Block, environment);
                condition = (bool)Evaluate(stmt.Condition);
            }

            return null;
        }

        public object VisitForStmt(Statement.ForStmt stmt)
        {
            stmt.Declaration.Accept(this);
            var condition = (bool)Evaluate(stmt.Condition);
            while (condition)
            {
                ExecuteBlock(stmt.Block, environment);
                stmt.Assignment.Accept(this);
                condition = (bool)Evaluate(stmt.Condition);
            }

            return null;
        }

        public object VisitBlockStmt(Statement.BlockStmt stmt) => ExecuteBlock(stmt, environment);

        public object VisitReturnStmt(Statement.ReturnStmt stmt) => Evaluate(stmt.Expr);

        public object VisitAssignmentStmt(Statement.AssignmentStmt stmt)
        {
            var variable = environment.Get(stmt.Variable.Text);

            environment.Set(stmt.Variable.Text, Evaluate(stmt.Value));
            return null;
        }

        public object VisitPrintStmt(Statement.PrintStmt stmt)
        {
            var val = Evaluate(stmt.Expr);
            var edited = val.ToString().Replace("True", "صحيح").Replace("False", "خطأ").Replace("null", "عدم");
            Writer.Write(edited);
            return null;
        }

        public static object ExecuteBlock(Statement.BlockStmt block, Environment env)
        {
            foreach (var binding in block.BindingList)
            {
                var localInterpret = new Interpreter(Writer, env);
                var result = binding.Accept(localInterpret);

                if (bindingIsReturn(binding)) return result;
            }

            return null;
        }

        public static void AddParamsToScope(List<Expression> parametersDef, List<Expression> parameters, Environment scope)
        {
            for (int i = 0; i < parametersDef.Count; i++)
            {
                var localInterpreter = new Interpreter(Writer, scope, true);
                var paramName = localInterpreter.Evaluate(parametersDef[i]).ToString();
                var paramValue = localInterpreter.Evaluate(parameters[i]);
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
