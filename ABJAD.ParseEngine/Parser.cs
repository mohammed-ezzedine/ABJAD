using ABJAD.Models;
using ABJAD.Models.Exceptions;
using static ABJAD.Models.TokenType;
using System.Collections.Generic;
using System;
using static ABJAD.Models.Constants;

namespace ABJAD.ParseEngine
{
    public class Parser
    {
        private readonly List<TokenType> declarationTokens = new List<TokenType> { CLASS, CONST, FUNC, VAR };
        private readonly List<Token> tokens;
        private List<Binding> program;

        private int _current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public List<Binding> Parse()
        {
            program = new List<Binding>();
            while (HasNext())
            {
                program.Add(Binding());
            }

            return program;
        }

        private bool HasNext()
        {
            if (_current >= tokens.Count)
            {
                return false;
            }

            return true;
        }

        private bool HasNext(out Token token)
        {
            if (_current >= tokens.Count)
            {
                token = null;
                return false;
            }

            token = tokens[_current++];
            return true;
        }

        /// <summary>
        /// Assumes that the type of the current token is one of the following <c>{ CLASS, CONST, FUNC, VAR }</c>
        /// </summary>
        /// <returns></returns>
        private Declaration Declaration()
        {
            switch (Consume().Type)
            {
                case CLASS:
                    return Class();
                case CONST:
                    return Constant();
                case FUNC:
                    return Function();
                case VAR:
                    return Variable();
                default:
                    return null;
            }
        }

        private Statement Statement()
        {
            switch (Peek()?.Type)
            {
                case OPEN_BRACE:
                    return Block();
                case FOR:
                    return ForStmt();
                case IF:
                    return IfStmt();
                case PRINT:
                    return PrintStmt();
                case RETURN:
                    return ReturnStmt();
                case WHILE:
                    return WhileStmt();
                default:
                    if (Peek(1)?.Type == EQUAL)
                    {
                        return AsstStmt();
                    }
                    else
                    {
                        return ExprStmt();
                    }
            }
        }

        private Declaration Class()
        {
            var name = Consume(ID, 
                ErrorMessages.English.ExpectedToken("class name", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("إسم الصنف", GetLine(), GetIndex()));

            var block = Block();
            
            return new Declaration.ClassDecl(name, block);
        }

        private Declaration Function()
        {
            var name = Consume(ID, 
                ErrorMessages.English.ExpectedToken("function name", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("إسم الدالة", GetLine(), GetIndex()));

            Consume(OPEN_PAREN, 
                ErrorMessages.English.ExpectedToken("(", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("(", GetLine(), GetIndex()));

            var parameters = Parameters();

            Consume(CLOSE_PAREN, 
                ErrorMessages.English.ExpectedToken(")", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken(")", GetLine(), GetIndex()));

            var block = Block();


            return new Declaration.FuncDecl(name, parameters, block);
        }

        private Declaration Constant()
        {
            var name = Consume(ID, 
                ErrorMessages.English.ExpectedToken("constant name", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("إسم الثابت", GetLine(), GetIndex()));

            Expression value = null;
            if (Peek()?.Type != SEMICOLON)
            {
                Consume(EQUAL, 
                    ErrorMessages.English.UnassignedConst(GetLine(), GetIndex()), 
                    ErrorMessages.Arabic.UnassignedConst(GetLine(), GetIndex()));

                value = Expression();
            }

            Consume(SEMICOLON, 
                ErrorMessages.English.ExpectedToken(";", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("؛", GetLine(), GetIndex()));

            return new Declaration.ConstDecl(name, value);
        }

        private Declaration Variable()
        {
            var name = Consume(ID, 
                ErrorMessages.English.ExpectedToken("variable name", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("إسم المتغير", GetLine(), GetIndex()));

            Expression value = null;
            if (Peek()?.Type != SEMICOLON)
            {
                Consume(EQUAL, 
                    ErrorMessages.English.UnassignedVar(GetLine(), GetIndex()), 
                    ErrorMessages.Arabic.UnassignedVar(GetLine(), GetIndex()));

                value = Expression();
            }

            Consume(SEMICOLON, 
                ErrorMessages.English.ExpectedToken(";", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("؛", GetLine(), GetIndex()));

            return new Declaration.VarDecl(name, value);
        }

        private Statement ExprStmt()
        {
            var expr = Expression();
            Consume(SEMICOLON, 
                ErrorMessages.English.ExpectedToken(";", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("؛", GetLine(), GetIndex()));

            return new Statement.ExprStmt(expr);
        }

        private Statement IfStmt()
        {
            Consume(IF, 
                ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()), 
                ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));

            Consume(OPEN_PAREN, 
                ErrorMessages.English.ExpectedToken("(", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("(", GetLine(), GetIndex()));

            var condition = Expression();

            Consume(CLOSE_PAREN, 
                ErrorMessages.English.ExpectedToken(")", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken(")", GetLine(), GetIndex()));

            Statement.BlockStmt ifBody = Block();
            Statement.BlockStmt elseBody = null;

            if (Peek()?.Type == ELSE)
            {
                Consume(ELSE, "", "");
                elseBody = Block();
            }

            return new Statement.IfStmt(condition, ifBody, elseBody);
        }

        private Statement WhileStmt()
        {
            Consume(WHILE, 
                ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()),
                ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));

            Consume(OPEN_PAREN, 
                ErrorMessages.English.ExpectedToken("(", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("(", GetLine(), GetIndex()));

            var condition = Expression();
            
            Consume(CLOSE_PAREN, 
                ErrorMessages.English.ExpectedToken(")", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken(")", GetLine(), GetIndex()));
            
            var body = Block();

            return new Statement.WhileStmt(condition, body);
        }

        private Statement ForStmt()
        {
            Consume(FOR, 
                ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()), 
                ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));

            Consume(OPEN_PAREN, 
                ErrorMessages.English.ExpectedToken("(", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("(", GetLine(), GetIndex()));

            Consume(VAR, 
                ErrorMessages.English.ForLoopVar(GetLine(), GetIndex()),
                ErrorMessages.Arabic.ForLoopVar(GetLine(), GetIndex()));

            var declaration = Variable();
            var condition = Expression();
            
            Consume(SEMICOLON, 
                ErrorMessages.English.ExpectedToken(";", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("؛", GetLine(), GetIndex()));

            var assignment = Assignment();
            
            Consume(CLOSE_PAREN, 
                ErrorMessages.English.ExpectedToken(")", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken(")", GetLine(), GetIndex()));

            var block = Block();

            return new Statement.ForStmt(block, assignment, condition, declaration);
        }

        private Statement ReturnStmt()
        {
            Consume(RETURN, 
                ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()), 
                ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));

            var expr = Expression();
            
            Consume(SEMICOLON, 
                ErrorMessages.English.ExpectedToken(";", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("؛", GetLine(), GetIndex()));

            return new Statement.ReturnStmt(expr);
        }

        private Statement AsstStmt()
        {
            var asst = Assignment();
            Consume(SEMICOLON, 
                ErrorMessages.English.ExpectedToken(";", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("؛", GetLine(), GetIndex()));

            return asst;
        }

        private Statement.AssignmentStmt Assignment()
        {
            var name = Consume(ID, 
                ErrorMessages.English.ExpectedToken("variable name", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("إسم المتغير", GetLine(), GetIndex()));

            Consume(EQUAL, 
                ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()), 
                ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));

            var value = Expression();

            return new Statement.AssignmentStmt(name, value);
        }

        private Statement.BlockStmt Block()
        {
            Consume(OPEN_BRACE, 
                ErrorMessages.English.ExpectedToken("{", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("{", GetLine(), GetIndex()));
            
            List<Binding> bindings = new List<Binding>();

            while (Peek()?.Type != CLOSE_BRACE)
            {
                bindings.Add(Binding());
            }

            Consume(CLOSE_BRACE, 
                ErrorMessages.English.ExpectedToken("}", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("}", GetLine(), GetIndex()));

            return new Statement.BlockStmt(bindings);
        }

        private Statement PrintStmt()
        {
            Consume(PRINT, 
                ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()), 
                ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));
            
            Consume(OPEN_PAREN, 
                ErrorMessages.English.ExpectedToken("(", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("(", GetLine(), GetIndex()));

            var expr = Expression();

            Consume(CLOSE_PAREN, 
                ErrorMessages.English.ExpectedToken(")", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken(")", GetLine(), GetIndex()));

            Consume(SEMICOLON, 
                ErrorMessages.English.ExpectedToken(";", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("؛", GetLine(), GetIndex()));

            return new Statement.PrintStmt(expr);
        }

        private List<Expression> Parameters()
        {
            var parameters = new List<Expression>();

            while (Peek()?.Type != CLOSE_PAREN)
            {
                parameters.Add(Expression());

                if(Peek()?.Type == COMMA) Consume();
            }

            return parameters;
        }

        private Expression Expression()
        {
            return Or();
        }

        private Expression Or()
        {
            return LeftPresedenceBinaryOperation(
                And,
                OR);
        }

        private Expression And()
        {
            return LeftPresedenceBinaryOperation(
                Compare,
                AND);
        }

        private Expression Compare()
        {
            return LeftPresedenceBinaryOperation(
                Addition,
                BANG_EQUAL, EQUAL_EQUAL, GREATER_EQUAL, GREATER_THAN, LESS_EQUAL, LESS_THAN);
        }

        private Expression Addition()
        {
            return LeftPresedenceBinaryOperation(
                Multiplication,
                PLUS, MINUS);
        }

        private Expression Multiplication()
        {
            return LeftPresedenceBinaryOperation(
                Modulo,
                TIMES, DIVIDED_BY);
        }

        private Expression Modulo()
        {
            return LeftPresedenceBinaryOperation(
                Unary,
                MODULO);
        }

        private Expression LeftPresedenceBinaryOperation(
            Func<Expression> higherPresedency,
            params TokenType[] tokens)
        {
            var expr = higherPresedency();

            while (Match(tokens))
            {
                var operand = Consume();
                var right = higherPresedency();
                expr = new Expression.BinaryExpr(expr, operand, right);
            }

            return expr;
        }

        private Expression Unary()
        {
            if (Match(MINUS, BANG))
            {
                var operand = Consume();
                var expression = PrimitiveExpr();
                return new Expression.UnaryExpr(operand, expression);
            }

            return PrimitiveExpr();
        }
        
        private Expression PrimitiveExpr()
        {
            if (Match(NEW)) return InstExpr();
            if (Match(ID) && Match(1, DOT, OPEN_PAREN)) return CallExpr();
            if (Match(OPEN_PAREN)) return GroupExpr();
            if (Match(STRING)) return ToStrExpr();
            if (Match(NUMBER)) return ToNumberExpr();
            if (Match(BOOL)) return ToBoolExpr();
            if (Match(TYPEOF)) return TypeofExpr();

            var primitive = Primitive();
            return new Expression.PrimitiveExpr(primitive);
        }

        private Primitive Primitive()
        {
            var current = Consume();

            switch (current.Type)
            {
                case FALSE:
                    return new Primitive.False();
                case NULL:
                    return new Primitive.Null();
                case TRUE:
                    return new Primitive.True();
                case NUMBER_CONST:
                    return new Primitive.NumberConst(current.Text);
                case ID:
                    return new Primitive.Identifier(current.Text);
                case STRING_CONST:
                    return new Primitive.StringConst(current.Text);
                default:
                    break;
            }

            throw new AbjadParsingException(
                ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()), 
                ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));
        }

        private Expression CallExpr()
        {
            Token className;

            if (Peek(1)?.Type == DOT)
            {
                className = Consume(ID, 
                    ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()), 
                    ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));

                Consume(DOT, "", "");

                if (!Match(1, OPEN_PAREN))
                {
                    var field = Consume(ID, 
                        ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()),
                        ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));

                    return new Expression.FieldExpr(className, field);
                }
            }
            else
            {
                className = null;
            }

            var funcName = Consume(ID, 
                ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()), 
                ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));

            Consume(OPEN_PAREN, 
                ErrorMessages.English.ExpectedToken("(", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("(", GetLine(), GetIndex()));

            var parameters = Parameters();

            Consume(CLOSE_PAREN, 
                ErrorMessages.English.ExpectedToken(")", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken(")", GetLine(), GetIndex()));

            return new Expression.CallExpr(className, funcName, parameters);
        }

        private Expression InstExpr()
        {
            Consume(NEW, 
                ErrorMessages.English.ExpectedToken("'new' keyword", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("'الكلمة المفتاح 'انشئ", GetLine(), GetIndex()));

            var className = Consume(ID, 
                ErrorMessages.English.ExpectedToken("class name", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("إسم الصنف", GetLine(), GetIndex()));

            Consume(OPEN_PAREN, 
                ErrorMessages.English.ExpectedToken("(", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("(", GetLine(), GetIndex()));

            var parameters = Parameters();

            Consume(CLOSE_PAREN, 
                ErrorMessages.English.ExpectedToken(")", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken(")", GetLine(), GetIndex()));

            return new Expression.InstExpr(className, parameters);
        }

        private Expression GroupExpr()
        {
            Consume(OPEN_PAREN, 
                ErrorMessages.English.ExpectedToken("(", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("(", GetLine(), GetIndex()));

            var expr = Expression();
            Consume(CLOSE_PAREN, 
                ErrorMessages.English.ExpectedToken(")", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken(")", GetLine(), GetIndex()));

            return new Expression.GroupExpr(expr);
        }

        private Expression ToStrExpr()
        {
            Consume(STRING, 
                ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()), 
                ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));

            Consume(OPEN_PAREN, 
                ErrorMessages.English.ExpectedToken("(", GetLine(), GetIndex()),
                ErrorMessages.Arabic.ExpectedToken("(", GetLine(), GetIndex()));

            var expr = Expression();
            Consume(CLOSE_PAREN, 
                ErrorMessages.English.ExpectedToken(")", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken(")", GetLine(), GetIndex()));

            return new Expression.ToStrExpr(expr);
        }

        private Expression ToNumberExpr()
        {
            Consume(NUMBER, 
                ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()), 
                ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));

            Consume(OPEN_PAREN, 
                ErrorMessages.English.ExpectedToken("(", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("(", GetLine(), GetIndex()));

            var expr = Expression();
            Consume(CLOSE_PAREN, 
                ErrorMessages.English.ExpectedToken(")", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken(")", GetLine(), GetIndex()));

            return new Expression.ToNumberExpr(expr);
        }

        private Expression ToBoolExpr()
        {
            Consume(BOOL, 
                ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()), 
                ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));

            Consume(OPEN_PAREN, 
                ErrorMessages.English.ExpectedToken("(", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("(", GetLine(), GetIndex()));

            var expr = Expression();
            Consume(CLOSE_PAREN, 
                ErrorMessages.English.ExpectedToken(")", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken(")", GetLine(), GetIndex()));

            return new Expression.ToBoolExpr(expr);
        }

        private Expression TypeofExpr()
        {
            Consume(TYPEOF, 
                ErrorMessages.English.InvalidSyntax(GetLine(), GetIndex()), 
                ErrorMessages.Arabic.InvalidSyntax(GetLine(), GetIndex()));

            Consume(OPEN_PAREN, 
                ErrorMessages.English.ExpectedToken("(", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken("(", GetLine(), GetIndex()));

            var expr = Expression();
            Consume(CLOSE_PAREN, 
                ErrorMessages.English.ExpectedToken(")", GetLine(), GetIndex()), 
                ErrorMessages.Arabic.ExpectedToken(")", GetLine(), GetIndex()));

            return new Expression.TypeofExpr(expr);
        }

        private Binding Binding()
        {
            Binding binding = null;
            Token token = Peek();

            if (token == null)
            {
                return null;
            }

            if (declarationTokens.Contains(token.Type))
            {
                var declaration = Declaration();
                binding = new Binding.DeclBinding(declaration);
            }
            else
            {
                var stmt = Statement();
                binding = new Binding.StmtBinding(stmt);
            }

            return binding;
        }

        private Token Consume(TokenType type, string errorMessage_en, string errorMessage_ar)
        {
            if (HasNext(out Token token))
            {
                if (token.Type == type)
                {
                    return token;
                }
            }
            _current--;

            throw new AbjadParsingException(errorMessage_en, errorMessage_ar);
        }

        private Token Consume()
        {
            if (HasNext(out Token token))
            {
                return token;
            }

            _current--;
            return null;
        }

        private bool Match(params TokenType[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if (Peek()?.Type == types[i])
                {
                    return true;
                }
            }

            return false;
        }

        private bool Match(int offset = 0, params TokenType[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if (Peek(offset)?.Type == types[i])
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// A function that's used to get a token ahead of the current one with some offset.
        /// It doesn't mutate the current global index
        /// </summary>
        /// <param name="offset">positive integer</param>
        /// <returns>token that's far offset steps from the current token</returns>
        private Token Peek(int offset = 0)
        {
            if (_current + offset >= tokens.Count)
                return null;

            return tokens[_current + offset];
        }

        private int GetLine()
        {
            return Peek().Line;
        }

        private int GetIndex()
        {
            return Peek().Index;
        }
    }
}
