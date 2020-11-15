using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.ParseEngine;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace ParserUnitTest
{
    class UnitTest2
    {
        private const string programPath = "test2.abjad";

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
        public void Parsing_FirstBinding_HasFourBindings()
        {
            var declBinding = Bindings[0] as Binding.DeclBinding;
            var classDecl = declBinding?.Decl as Declaration.ClassDecl;
            var block = classDecl.Block as Statement.BlockStmt;
            Assert.AreEqual(4, block.BindingList.Count);
        }

        [Test]
        public void Parsing_FirstBindingThirdChild_TypeIsFunction()
        {
            var declBinding = Bindings[0] as Binding.DeclBinding;
            var classDecl = declBinding?.Decl as Declaration.ClassDecl;
            var block = classDecl.Block as Statement.BlockStmt;
            var bindingDecl = block.BindingList[2] as Binding.DeclBinding;
            var funcDecl = bindingDecl.Decl as Declaration.FuncDecl;
            Assert.NotNull(funcDecl);
        }

        [Test]
        public void Parsing_FirstBindingThirdChild_TakesTwoParameters()
        {
            var declBinding = Bindings[0] as Binding.DeclBinding;
            var classDecl = declBinding?.Decl as Declaration.ClassDecl;
            var block = classDecl.Block as Statement.BlockStmt;
            var bindingDecl = block.BindingList[2] as Binding.DeclBinding;
            var funcDecl = bindingDecl.Decl as Declaration.FuncDecl;
            Assert.AreEqual(2, funcDecl.Parameters.Count);
        }

        [Test]
        public void Parsing_FirstBindingForthChild_ReturnsValue()
        {
            var declBinding = Bindings[0] as Binding.DeclBinding;
            var classDecl = declBinding?.Decl as Declaration.ClassDecl;
            var block = classDecl.Block as Statement.BlockStmt;
            var bindingDecl = block.BindingList[3] as Binding.DeclBinding;
            var funcDecl = bindingDecl.Decl as Declaration.FuncDecl;
            var lastBinding = funcDecl.Block.BindingList[1] as Binding.StmtBinding;
            var returnStmt = lastBinding.Stmt as Statement.ReturnStmt;
            Assert.NotNull(returnStmt);
        }
    }
}