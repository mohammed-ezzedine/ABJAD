using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using static ABJAD.Models.TokenType;

namespace LexerUnitTest
{
    class UnitTest2
    {
        private const string programPath = "test2.abjad";
        private List<Token> Tokens;

        [SetUp]
        public void Setup()
        {
            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var dir = Path.Combine(rootDir, programPath);

            var code = Reader.Read(dir);

            var lexer = new Lexer(code);
            Tokens = lexer.Lex();
        }

        [Test]
        public void Lexing_ForthToken_IsVar()
        {
            Assert.AreEqual(VAR, Tokens[3].Type);
        }

        [Test]
        public void Lexing_SeventhToken_IsNull()
        {
            Assert.AreEqual(NULL, Tokens[6].Type);
        }

        [Test]
        public void Lexing_FortyThirdToken_IsReturn()
        {
            Assert.AreEqual(RETURN, Tokens[42].Type);
        }
    }
}
