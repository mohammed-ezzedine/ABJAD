using ABJAD.LexEngine;
using ABJAD.Models;
using NUnit.Framework;
using System.Linq;

namespace LexerUnitTest
{
    class KeywordsModificationUnitTest
    {
        [Test]
        public void Lexing_NewToken_Syntax()
        {
            var code = "متغير هذا = انشئ ملف()؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var newToken = tokens
                .SingleOrDefault(t => t.Type == TokenType.NEW);

            Assert.NotNull(newToken);
        }

        [Test]
        public void Lexing_NewToken_Syntax1()
        {
            var code = "انشئ؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var newToken = tokens[0];

            Assert.AreEqual(TokenType.NEW, newToken.Type);
        }

        [Test]
        public void Lexing_ForToken_Syntax()
        {
            var code = "كرر؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var forToken = tokens[0];

            Assert.AreEqual(TokenType.FOR, forToken.Type);
        }

        [Test]
        public void Lexing_ForToken_Syntax1()
        {
            var code = "كرر (متغير ا = 0؛ ا > 10؛ ا++) { اكتب(ا)؛}";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var forToken = tokens[0];

            Assert.AreEqual(TokenType.FOR, forToken.Type);
        }

        [Test]
        public void Lexing_ElseToken_Syntax()
        {
            var code = "والا؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var elseToken = tokens[0];

            Assert.AreEqual(TokenType.ELSE, elseToken.Type);
        }

        [Test]
        public void Lexing_ElseToken_Syntax1()
        {
            var code = "اذا (خطا) { اكتب(0)؛ } والا { اكتب(1)؛ }";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var elseToken = tokens
                .SingleOrDefault(t => t.Type == TokenType.ELSE);

            Assert.NotNull(elseToken);
        }
    }
}
