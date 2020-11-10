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
                program.Add(ConsumeBinding());
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

        private Primitive ConsumePrimitive()
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

        /// <summary>
        /// Assumes that the type of the current token is one of the following <c>{ CLASS, CONST, FUNC, VAR }</c>
        /// </summary>
        /// <returns></returns>
        private Declaration ConsumeDeclaration()
        {
            switch (Consume().Type)
            {
                case CLASS:
                    return ConsumeClass();
                case CONST:
                    return ConsumeConst();
                case FUNC:
                    return ConsumeFunction();
                case VAR:
                    return ConsumeVar();
                default:
                    return null;
            }
        }

        private Statement ConsumeStatement()
        {
            switch (Peek().Type)
            {
                case OPEN_BRACE:
                    return ConsumeBlock();
                case FOR:
                    return ConsumeForStmt();
                case IF:
                    return ConsumeIfStmt();
                case PRINT:
                    return ConsumePrintStmt();
                case RETURN:
                    return ConsumeReturnStmt();
                case WHILE:
                    return ConsumeWhileStmt();
                default:
                    if (Peek(1).Type == EQUAL)
                    {
                        return ConsumeAsstStmt();
                    }
                    else
                    {
                        return ConsumeExprStmt();
                    }
            }
        }

        private Declaration ConsumeClass()
        {
            var name = Consume(ID, "The class name should go directly after the class keyword.");

            var block = ConsumeBlock();
            
            return new Declaration.ClassDecl(name, block);
        }

        private Declaration ConsumeFunction()
        {
            var name = Consume(ID, "The function name should go directly after the function keyword.");

            Consume(OPEN_PAREN, "A function should be followed by parenthesis");

            var parameters = ConsumeParameters();

            Consume(CLOSE_PAREN, "A function parameters should be followed by a closing ')'");

            var block = ConsumeBlock();


            return new Declaration.FuncDecl(name, parameters, block);
        }

        private Declaration ConsumeConst()
        {
            var name = Consume(ID, "The constant should have a name.");

            Consume(EQUAL, "A constant shall not be declared without a value");

            var value = ConsumeExpression();

            Consume(SEMICOLON, $"The declaration of the constant {name.Text} is missing a semicolon.");

            return new Declaration.ConstDecl(name, value);
        }

        private Declaration ConsumeVar()
        {
            var name = Consume(ID, "The variable should have a name.");

            Consume(EQUAL, "A variable shall not be declared without a value");

            var value = ConsumeExpression();

            Consume(SEMICOLON, $"The declaration of the variable {name.Text} is missing a semicolon.");

            return new Declaration.VarDecl(name, value);
        }

        private Statement ConsumeExprStmt()
        {
            var expr = ConsumeExpression();
            Consume(SEMICOLON, "A statement should end with a semicolon.");

            return new Statement.ExprStmt(expr);
        }

        private Statement ConsumeIfStmt()
        {
            Consume(IF, "Wrong if statement syntax.");
            Consume(OPEN_PAREN, "The condition of the if statement should be enclosed with paranthesis.");

            var condition = ConsumeExpression();

            Consume(CLOSE_PAREN, "A closing ')' is missing.");

            Statement.BlockStmt ifBody = ConsumeBlock();
            Statement.BlockStmt elseBody = null;

            if (Peek().Type == ELSE)
            {
                Consume(ELSE, "");
                elseBody = ConsumeBlock();
            }

            return new Statement.IfStmt(condition, ifBody, elseBody);
        }

        private Statement ConsumeWhileStmt()
        {
            Consume(WHILE, "Wrong while statement syntax.");
            Consume(OPEN_PAREN, "The condition of the while statement should be enclosed with paranthesis.");
            var condition = ConsumeExpression();
            Consume(CLOSE_PAREN, "A closing ')' is missing.");
            var body = ConsumeBlock();

            return new Statement.WhileStmt(condition, body);
        }

        private Statement ConsumeForStmt()
        {
            Consume(FOR, "Wrong for statament syntax.");
            Consume(OPEN_PAREN, "For keyword should be followed by an opening '('");
            var declaration = ConsumeVar();
            var condition = ConsumeExpression();
            Consume(SEMICOLON, "Statement should be seperated by a semicolon.");
            var assignment = ConsumeAssignment();
            Consume(CLOSE_PAREN, "A closing ')' is missing.");
            var block = ConsumeBlock();

            return new Statement.ForStmt(block, assignment, condition, declaration);
        }

        private Statement ConsumeReturnStmt()
        {
            Consume(RETURN, "Wrong return stament syntax.");
            var expr = ConsumeExpression();
            Consume(SEMICOLON, "A statement should end with a semicolon.");

            return new Statement.ReturnStmt(expr);
        }

        private Statement ConsumeAsstStmt()
        {
            var asst = ConsumeAssignment();
            Consume(SEMICOLON, "A statement should end with a semicolon.");

            return asst;
        }

        private Statement.AssignmentStmt ConsumeAssignment()
        {
            var name = Consume(ID, "Variable name was expected.");
            Consume(EQUAL, "Wrong assignment statement syntax.");
            var value = ConsumeExpression();

            return new Statement.AssignmentStmt(name, value);
        }

        private Statement.BlockStmt ConsumeBlock()
        {
            Consume(OPEN_BRACE, "A block should be enclosed with braces.");
            
            List<Binding> bindings = new List<Binding>();

            while (Peek().Type != CLOSE_BRACE)
            {
                bindings.Add(ConsumeBinding());
            }

            Consume(CLOSE_BRACE, "A closing '}' is missing.");

            return new Statement.BlockStmt(bindings);
        }

        private Statement ConsumePrintStmt()
        {
            Consume(PRINT, "Wrong print stament syntax.");
            Consume(OPEN_PAREN, "The expression should be enclosed with paranthesis.");
            var expr = ConsumeExpression();
            Consume(CLOSE_PAREN, "A closing ')' is missing.");
            Consume(SEMICOLON, "A statement should end with a semicolon.");

            return new Statement.PrintStmt(expr);
        }

        private List<Expression> ConsumeParameters()
        {
            var parameters = new List<Expression>();

            while (Peek().Type != CLOSE_PAREN)
            {
                parameters.Add(ConsumeExpression());

                if(Peek().Type == COMMA) Consume();
            }

            return parameters;
        }

        private Expression ConsumeExpression()
        {
            //if (HasNext(out Token token))
            //{
            if (_current == 13)
            {

            }
            var token = Peek();
                switch (token.Type)
                {
                    case FALSE:
                    case NULL:
                    case TRUE:
                    case NUMBER_CONST:
                    case STRING_CONST:
                        return ConsumePrimitiveExpr();
                    case NEW:
                        return ConsumeInstExpr();
                    case ID:
                        var nextToken = Peek(1);
                        if (nextToken.Type == DOT || nextToken.Type == OPEN_PAREN)
                        {
                            return ConsumeCallExpr();
                        }
                        else
                        {
                            return ConsumeOperExpr();
                        }
                    case BANG:
                    case MINUS:
                        return ConsumeOperExpr();
                    default:
                    // TODO
                        try
                        {
                            return ConsumeOperExpr();
                        }
                        catch (AbjadParsingException)
                        {
                            return ConsumeGroupExpr();
                        }
                }
            //}

            //return null;
        }

        private Expression ConsumeOperExpr()
        {
            Token operation;

            var currentToken = Consume();
            if (currentToken.Type == BANG || currentToken.Type == MINUS)
            {
                operation = Consume();
                var expr = ConsumeExpression();
                return new Expression.UnaryExpr(operation, expr);
            }

            var operand1 = ConsumeExpression();
            operation = ConsumeBinaryOper();
            var operand2 = ConsumeExpression();

            return new Expression.BinaryExpr(operand1, operation, operand2);
        }

        //private Expression ConsumeBOperExpr()
        //{
        //    var operand = Consume();
        //    if (!binaryOperations.Contains(operand.Type))
        //    {
        //        throw new AbjadParsingException("Invalid binary operation syntax.");
        //    }


        //}

        private Expression ConsumeCallExpr()
        {
            Token className;

            if (Peek(1).Type == DOT)
            {
                className = Consume(ID, "Invalid class name syntax.");
                Consume(DOT, "");
            }
            else
            {
                className = null;
            }

            var funcName = Consume(ID, "Invalid function name syntax.");
            Consume(OPEN_PAREN, "A function name should be followed by parenthesis");

            var parameters = ConsumeParameters();

            Consume(CLOSE_PAREN, "function parameters should be followed by a closing ')'");
            //Consume(SEMICOLON, "A function call should be followed by a semicolon.");

            return new Expression.CallExpr(className, funcName, parameters);
        }

        private Expression ConsumeInstExpr()
        {
            Consume(NEW, "A new keyword is required for instantiating a class.");
            var className = Consume(ID, "class name is required.");
            Consume(OPEN_PAREN, "A class name should be followed by parenthesis");

            var parameters = ConsumeParameters();

            Consume(CLOSE_PAREN, "Class parameters should be followed by a closing ')'");
            //Consume(SEMICOLON, "An expression should be followed by a semicolon.");

            return new Expression.InstExpr(className, parameters);
        }

        private Expression ConsumePrimitiveExpr()
        {
            var primitive = ConsumePrimitive();
            return new Expression.PrimitiveExpr(primitive);
        }

        private Expression ConsumeGroupExpr()
        {
            Consume(OPEN_PAREN, "'(' was expected.");
            var expr = ConsumeExpression();
            Consume(CLOSE_PAREN, "')' was expected.");

            return new Expression.GroupExpr(expr);
        }

        private Binding ConsumeBinding()
        {
            Binding binding = null;
            Token token = Peek();

            if (token == null)
            {
                return null;
            }

            if (declarationTokens.Contains(token.Type))
            {
                var declaration = ConsumeDeclaration();
                binding = new Binding.DeclBinding(declaration);
            }
            else
            {
                var stmt = ConsumeStatement();
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

        private Token ConsumeBinaryOper()
        {
            var binaryTypes = new List<TokenType>() { PLUS, MINUS, TIMES, DIVIDED_BY, AND, OR, EQUAL_EQUAL, GREATER_EQUAL, GREATER_THAN, LESS_EQUAL, LESS_THAN, BANG_EQUAL };
            if (HasNext(out Token token))
            {
                if (binaryTypes.Contains(token.Type))
                {
                    return token;
                }
            }
            _current--;

            throw new AbjadParsingException("Invalid binary operation");
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
