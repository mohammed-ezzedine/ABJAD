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

        [Test]
        public void Lexing_StringToken_Syntax()
        {
            var code = "مقطع؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            Assert.AreEqual(TokenType.STRING, tokens[0].Type);
        }

        [Test]
        public void Lexing_StringToken_Syntax1()
        {
            var code = "أكتب(مقطع(3))؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var stringToken = tokens
                .SingleOrDefault(t => t.Type == TokenType.STRING);

            Assert.NotNull(stringToken);
        }

        [Test]
        public void Lexing_NumberToken_Syntax()
        {
            var code = "رقم؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            Assert.AreEqual(TokenType.NUMBER, tokens[0].Type);
        }

        [Test]
        public void Lexing_NumberToken_Syntax1()
        {
            var code = "أكتب(رقم(\"3\"))؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var numberToken = tokens
                .SingleOrDefault(t => t.Type == TokenType.NUMBER);

            Assert.NotNull(numberToken);
        }

        [Test]
        public void Lexing_BoolToken_Syntax()
        {
            var code = "منطق؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            Assert.AreEqual(TokenType.BOOL, tokens[0].Type);
        }

        [Test]
        public void Lexing_BoolToken_Syntax1()
        {
            var code = "أكتب(منطق(\"صحيح\"))؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var boolToken = tokens
                .SingleOrDefault(t => t.Type == TokenType.BOOL);

            Assert.NotNull(boolToken);
        }

        [Test]
        public void Lexing_TypeofToken_Syntax()
        {
            var code = "نوع؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            Assert.AreEqual(TokenType.TYPEOF, tokens[0].Type);
        }

        [Test]
        public void Lexing_TypeofToken_Syntax1()
        {
            var code = "أكتب(نوع(صحيح))؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var typeToken = tokens
                .SingleOrDefault(t => t.Type == TokenType.TYPEOF);

            Assert.NotNull(typeToken);
        }
    }
}
