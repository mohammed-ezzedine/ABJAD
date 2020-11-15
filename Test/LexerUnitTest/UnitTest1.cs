using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static ABJAD.Models.TokenType;

namespace LexerUnitTest
{
    class UnitTest1
    {
        private const string programPath = "test1.abjad";
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
        public void Lexing_Tokens_NonEmpty()
        {
            Assert.IsNotEmpty(Tokens);
        }

        [Test]
        public void Lexing_FirstToken_TypeClass()
        {
            Assert.AreEqual(CLASS, Tokens[0].Type);
        }

        [Test]
        public void Lexing_Tokens_ContainsPrint()
        {
            Assert.Contains(PRINT, Tokens.Select(t => t.Type).ToList());
        }

        [Test]
        public void Lexing_Tokens_ContainsNumber()
        {
            Assert.Contains(NUMBER_CONST, Tokens.Select(t => t.Type).ToList());
        }

        [Test]
        public void Lexing_Semicolons_AreThree()
        {
            Assert.AreEqual(3, Tokens.Where(t => t.Type == SEMICOLON).Count());
        }

        [Test]
        public void Lexing_Tokens_Are35()
        {
            Assert.AreEqual(35, Tokens.Count());
        }
    }
}