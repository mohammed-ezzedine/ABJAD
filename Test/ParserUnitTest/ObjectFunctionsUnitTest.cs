using ABJAD.LexEngine;
using ABJAD.ParseEngine;
using NUnit.Framework;

namespace ParserUnitTest
{
    class ObjectFunctionsUnitTest
    {
        [Test]
        public void Parsing_ToStrExpr_NotNull()
        {
            var code = "أكتب(مقطع(خطا))؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();
            var stmtBinding = bindings[0] as Binding.StmtBinding;
            var printStmt = stmtBinding.Stmt as Statement.PrintStmt;
            var toStrExpr = printStmt.Expr as Expression.ToStrExpr;

            Assert.NotNull(toStrExpr);
        }

        [Test]
        public void Parsing_ToNumberExpr_NotNull()
        {
            var code = "أكتب(رقم(\"3\"))؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();
            var stmtBinding = bindings[0] as Binding.StmtBinding;
            var printStmt = stmtBinding.Stmt as Statement.PrintStmt;
            var toNumberExpr = printStmt.Expr as Expression.ToNumberExpr;

            Assert.NotNull(toNumberExpr);
        }

        [Test]
        public void Parsing_ToBoolExpr_NotNull()
        {
            var code = "أكتب(منطق(\"صحيح\"))؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();
            var stmtBinding = bindings[0] as Binding.StmtBinding;
            var printStmt = stmtBinding.Stmt as Statement.PrintStmt;
            var toBoolExpr = printStmt.Expr as Expression.ToBoolExpr;

            Assert.NotNull(toBoolExpr);
        }

        [Test]
        public void Parsing_TypeofExpr_NotNull()
        {
            var code = "أكتب(نوع(\"صحيح\"))؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();
            var stmtBinding = bindings[0] as Binding.StmtBinding;
            var printStmt = stmtBinding.Stmt as Statement.PrintStmt;
            var typeofExpr = printStmt.Expr as Expression.TypeofExpr;

            Assert.NotNull(typeofExpr);
        }
    }
}
