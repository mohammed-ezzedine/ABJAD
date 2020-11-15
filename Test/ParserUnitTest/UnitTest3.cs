using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.ParseEngine;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace ParserUnitTest
{
    class UnitTest3
    {
        private const string programPath = "test6.abjad";

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
        public void Parsing_SecondBinding_TypeIsWhileStmt()
        {
            var stmtBinding = Bindings[1] as Binding.StmtBinding;
            var whileStmt = stmtBinding.Stmt as Statement.WhileStmt;
            Assert.NotNull(whileStmt);
        }
    }
}