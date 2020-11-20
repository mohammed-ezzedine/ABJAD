using ABJAD.LexEngine;
using ABJAD.Models;
using NUnit.Framework;
using System.Linq;

namespace LexerUnitTest
{
    class CaseSensitivityTest
    {
        [Test]
        public void Lexing_Tokens_AreCaseInsensitive()
        {
            var code = "متغير هذا = 1؛اكتب(هـــذا)؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var ids = tokens
                .Where(t => t.Type == TokenType.ID)
                .ToList();

            Assert.AreEqual(2, ids.Count, message: "The should be 2 IDs in the token list");
            Assert.True(ids[0].Text == ids[1].Text);
        }

        [Test]
        public void Lexing_Tokens_AreCaseInsensitive2()
        {
            var code = "متغير هــذا = 1؛اكتب(هـــذا)؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var ids = tokens
                .Where(t => t.Type == TokenType.ID)
                .ToList();

            Assert.AreEqual(2, ids.Count, message: "The should be 2 IDs in the token list");
            Assert.True(ids[0].Text == ids[1].Text, $"{ids[0].Text} {ids[1].Text}");
        }

        [Test]
        public void Lexing_Tokens_AreCaseInsensitive3()
        {
            var code = "متغير ـهذا = 1؛اكتب(هـذا)؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var ids = tokens
                .Where(t => t.Type == TokenType.ID)
                .ToList();

            Assert.AreEqual(2, ids.Count, message: "The should be 2 IDs in the token list");
            Assert.True(ids[0].Text == ids[1].Text, $"{ids[0].Text} {ids[1].Text}");
        }

        [Test]
        public void Lexing_IfStatements_AreCaseInsensitive()
        {
            var code = "اذا(صحيح) {اكتب(1)؛}";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            Assert.AreEqual(TokenType.IF, tokens[0].Type);
        }

        [Test]
        public void Lexing_PrintStatements_AreCaseInsensitive()
        {
            var code = "اكتب(1)؛";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            Assert.AreEqual(TokenType.PRINT, tokens[0].Type);
        }
    }
}
