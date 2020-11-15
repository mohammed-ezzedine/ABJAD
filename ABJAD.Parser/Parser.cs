using ABJAD.Models;
using ABJAD.Models.Exceptions;
using static ABJAD.Models.TokenType;
using System.Collections.Generic;
using System;

namespace ABJAD.Parser
{
    public class Parser
    {
        private readonly List<TokenType> declarationTokens = new List<TokenType> { CLASS, CONST, FUNC, VAR };
        private readonly List<TokenType> binaryOperations = new List<TokenType> { AND, BANG_EQUAL, DIVIDED_BY, EQUAL_EQUAL, GREATER_EQUAL,
            GREATER_THAN, LESS_EQUAL, LESS_THAN, MINUS, OR, PLUS, TIMES };
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
            var name = Consume(ID, "The class name should go directly after the class keyword.");

            var block = Block();
            
            return new Declaration.ClassDecl(name, block);
        }

        private Declaration Function()
        {
            var name = Consume(ID, "The function name should go directly after the function keyword.");

            Consume(OPEN_PAREN, "A function should be followed by parenthesis");

            var parameters = Parameters();

            Consume(CLOSE_PAREN, "A function parameters should be followed by a closing ')'");

            var block = Block();


            return new Declaration.FuncDecl(name, parameters, block);
        }

        private Declaration Constant()
        {
            var name = Consume(ID, "The constant should have a name.");

            Expression value = null;
            if (Peek()?.Type != SEMICOLON)
            {
                Consume(EQUAL, "A constant shall not be declared without a value");

                value = Expression();
            }

            Consume(SEMICOLON, $"The declaration of the constant {name.Text} is missing a semicolon.");

            return new Declaration.ConstDecl(name, value);
        }

        private Declaration Variable()
        {
            var name = Consume(ID, "The variable should have a name.");

            Expression value = null;
            if (Peek()?.Type != SEMICOLON)
            {
                Consume(EQUAL, "A variable shall not be declared without a value");

                value = Expression();
            }

            Consume(SEMICOLON, $"The declaration of the variable {name.Text} is missing a semicolon.");

            return new Declaration.VarDecl(name, value);
        }

        private Statement ExprStmt()
        {
            var expr = Expression();
            Consume(SEMICOLON, "A statement should end with a semicolon.");

            return new Statement.ExprStmt(expr);
        }

        private Statement IfStmt()
        {
            Consume(IF, "Wrong if statement syntax.");
            Consume(OPEN_PAREN, "The condition of the if statement should be enclosed with paranthesis.");

            var condition = Expression();

            Consume(CLOSE_PAREN, "A closing ')' is missing.");

            Statement.BlockStmt ifBody = Block();
            Statement.BlockStmt elseBody = null;

            if (Peek()?.Type == ELSE)
            {
                Consume(ELSE, "");
                elseBody = Block();
            }

            return new Statement.IfStmt(condition, ifBody, elseBody);
        }

        private Statement WhileStmt()
        {
            Consume(WHILE, "Wrong while statement syntax.");
            Consume(OPEN_PAREN, "The condition of the while statement should be enclosed with paranthesis.");
            var condition = Expression();
            Consume(CLOSE_PAREN, "A closing ')' is missing.");
            var body = Block();

            return new Statement.WhileStmt(condition, body);
        }

        private Statement ForStmt()
        {
            Consume(FOR, "Wrong for statament syntax.");
            Consume(OPEN_PAREN, "For keyword should be followed by an opening '('");
            Consume(VAR, "Variables only can be used in for loops.");
            var declaration = Variable();
            var condition = Expression();
            Consume(SEMICOLON, "Statement should be seperated by a semicolon.");
            var assignment = Assignment();
            Consume(CLOSE_PAREN, "A closing ')' is missing.");
            var block = Block();

            return new Statement.ForStmt(block, assignment, condition, declaration);
        }

        private Statement ReturnStmt()
        {
            Consume(RETURN, "Wrong return stament syntax.");
            var expr = Expression();
            Consume(SEMICOLON, "A statement should end with a semicolon.");

            return new Statement.ReturnStmt(expr);
        }

        private Statement AsstStmt()
        {
            var asst = Assignment();
            Consume(SEMICOLON, "A statement should end with a semicolon.");

            return asst;
        }

        private Statement.AssignmentStmt Assignment()
        {
            var name = Consume(ID, "Variable name was expected.");
            Consume(EQUAL, "Wrong assignment statement syntax.");
            var value = Expression();

            return new Statement.AssignmentStmt(name, value);
        }

        private Statement.BlockStmt Block()
        {
            Consume(OPEN_BRACE, "A block should be enclosed with braces.");
            
            List<Binding> bindings = new List<Binding>();

            while (Peek()?.Type != CLOSE_BRACE)
            {
                bindings.Add(Binding());
            }

            Consume(CLOSE_BRACE, "A closing '}' is missing.");

            return new Statement.BlockStmt(bindings);
        }

        private Statement PrintStmt()
        {
            Consume(PRINT, "Wrong print stament syntax.");
            Consume(OPEN_PAREN, "The expression should be enclosed with paranthesis.");
            var expr = Expression();
            Consume(CLOSE_PAREN, "A closing ')' is missing.");
            Consume(SEMICOLON, "A statement should end with a semicolon.");

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
                Unary,
                TIMES, DIVIDED_BY);
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

            throw new AbjadParsingException("Invalid primitive syntax.");
        }

        private Expression CallExpr()
        {
            Token className;

            if (Peek(1)?.Type == DOT)
            {
                className = Consume(ID, "Invalid class name syntax.");
                Consume(DOT, "");

                if (!Match(1, OPEN_PAREN))
                {
                    var field = Consume(ID, "Invalid field syntax.");

                    return new Expression.FieldExpr(className, field);
                }
            }
            else
            {
                className = null;
            }

            var funcName = Consume(ID, "Invalid function name syntax.");
            Consume(OPEN_PAREN, "A function name should be followed by parenthesis");

            var parameters = Parameters();

            Consume(CLOSE_PAREN, "function parameters should be followed by a closing ')'");

            return new Expression.CallExpr(className, funcName, parameters);
        }

        private Expression InstExpr()
        {
            Consume(NEW, "A new keyword is required for instantiating a class.");
            var className = Consume(ID, "class name is required.");
            Consume(OPEN_PAREN, "A class name should be followed by parenthesis");

            var parameters = Parameters();

            Consume(CLOSE_PAREN, "Class parameters should be followed by a closing ')'");

            return new Expression.InstExpr(className, parameters);
        }

        private Expression GroupExpr()
        {
            Consume(OPEN_PAREN, "'(' was expected.");
            var expr = Expression();
            Consume(CLOSE_PAREN, "')' was expected.");

            return new Expression.GroupExpr(expr);
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

        private Token Consume(TokenType type, string errorMessage)
        {
            if (HasNext(out Token token))
            {
                if (token.Type == type)
                {
                    return token;
                }
            }
            _current--;

            throw new AbjadParsingException(errorMessage);
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
    }
}
