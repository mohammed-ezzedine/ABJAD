using ABJAD.InterpretEngine;
using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.ParseEngine;
using NUnit.Framework;
using System.IO;

namespace InterpreterUnitTest
{
    class UnitTest4
    {
        private const string programPath = "test4.abjad";
        private const string expectedOutput = "-1\n4\n";
        private string outputPath;

        [SetUp]
        public void Setup()
        {
            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var dir = Path.Combine(rootDir, programPath);

            outputPath = Path.Combine(rootDir, programPath.Split(".abjad")[0] + ".txt");

            var code = Reader.Read(dir);

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new Writer(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);
        }

        [Test]
        public void Interpreting_OutputFile_ExpectedContent()
        {
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }
    }
}