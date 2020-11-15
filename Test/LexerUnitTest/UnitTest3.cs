using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using static ABJAD.Models.TokenType;

namespace LexerUnitTest
{
    class UnitTest3
    {
        private const string programPath = "test3.abjad";
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
        public void Lexing_ForthToken_IsNumber()
        {
            Assert.AreEqual(NUMBER_CONST, Tokens[3].Type);
        }

        [Test]
        public void Lexing_FifthToken_IsPlus()
        {
            Assert.AreEqual(PLUS, Tokens[4].Type);
        }

        [Test]
        public void Lexing_EleventhToken_IsDividedby()
        {
            Assert.AreEqual(DIVIDED_BY, Tokens[10].Type);
        }
    }
}
