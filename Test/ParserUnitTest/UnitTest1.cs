using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.ParseEngine;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace ParserUnitTest
{
    class UnitTest1
    {
        private const string programPath = "test1.abjad";

        private List<Binding> Bindings;

        [SetUp]
        public void Setup()
        {
            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var dir = Path.Combine(rootDir, programPath);

            var code = Reader.Read(dir);

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            Bindings = parser.Parse();
        }

        [Test]
        public void Parsing_Bindings_AreThree()
        {
            Assert.AreEqual(3, Bindings.Count);
        }

        [Test]
        public void Parsing_FirstBinding_TypeIsClass()
        {
            var declBinding = Bindings[0] as Binding.DeclBinding;
            var classDecl = declBinding?.Decl as Declaration.ClassDecl;
            Assert.NotNull(classDecl);
        }

        [Test]
        public void Parsing_FirstBinding_HasTwoBindings()
        {
            var declBinding = Bindings[0] as Binding.DeclBinding;
            var classDecl = declBinding?.Decl as Declaration.ClassDecl;
            Assert.AreEqual(2, ((Statement.BlockStmt)classDecl.Block).BindingList.Count);
        }

        [Test]
        public void Parsing_SecondBinding_TypeIsVarDecl()
        {
            var declBinding = Bindings[1] as Binding.DeclBinding;
            var varDecl = declBinding?.Decl as Declaration.VarDecl;
            Assert.NotNull(varDecl);
        }

        [Test]
        public void Parsing_SecondBinding_ValueOfTypeInstExpr()
        {
            var declBinding = Bindings[1] as Binding.DeclBinding;
            var varDecl = declBinding?.Decl as Declaration.VarDecl;
            var val = varDecl.Value as Expression.InstExpr;
            Assert.NotNull(val);
        }

        [Test]
        public void Parsing_ThirdBinding_TypeIsCallExpr()
        {
            var stmtBinding = Bindings[2] as Binding.StmtBinding;
            var exprStmt = stmtBinding?.Stmt as Statement.ExprStmt;
            var callExpr = exprStmt.Expr as Expression.CallExpr;
            Assert.NotNull(callExpr);
        }

        [Test]
        public void Parsing_ThirdBindingCallExpr_TakesNoParameters()
        {
            var stmtBinding = Bindings[2] as Binding.StmtBinding;
            var exprStmt = stmtBinding?.Stmt as Statement.ExprStmt;
            var callExpr = exprStmt.Expr as Expression.CallExpr;
            Assert.AreEqual(0, callExpr.Parameters.Count);
        }
    }
}