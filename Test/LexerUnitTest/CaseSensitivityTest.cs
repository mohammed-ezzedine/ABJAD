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
            var code = "متغير هذا = 1؛أكتب(هـــذا)";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var ids = tokens
                .Where(t => t.Type == TokenType.ID)
                .ToList();

            Assert.True(ids[0].Text == ids[1].Text);
        }

        [Test]
        public void Lexing_Tokens_AreCaseInsensitive2()
        {
            var code = "متغير هــذا = 1؛أكتب(هـــذا)";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var ids = tokens
                .Where(t => t.Type == TokenType.ID)
                .ToList();

            Assert.True(ids[0].Text == ids[1].Text);
        }

        [Test]
        public void Lexing_Tokens_AreCaseInsensitive3()
        {
            var code = "متغير ـهذا = 1؛أكتب(هـذا)";
            var lexer = new Lexer(code);
            var tokens = lexer.Lex();
            var ids = tokens
                .Where(t => t.Type == TokenType.ID)
                .ToList();

            Assert.True(ids[0].Text == ids[1].Text);
        }
    }
}
