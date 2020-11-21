using ABJAD.InterpretEngine;
using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.Models.Exceptions;
using ABJAD.ParseEngine;
using NUnit.Framework;
using System.IO;

namespace InterpreterUnitTest
{
    class ScopesUnitTest
    {
        [Test]
        public void Scopes_AccessingVariablesFromDifferentScope_ExpectException()
        {
            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var dir = Path.Combine(rootDir, "animal.abjad");

            var outputPath = Path.Combine(rootDir, "animal.txt");

            var code = Reader.Read(dir);

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new Writer(outputPath);
            var interpreter = new Interpreter(writer);
            Assert.Throws(typeof(AbjadInterpretingException), new TestDelegate(() => interpreter.Interpret(bindings)));
        }
    }
}
