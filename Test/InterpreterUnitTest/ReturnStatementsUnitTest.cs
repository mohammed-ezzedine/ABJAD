using ABJAD.InterpretEngine;
using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.ParseEngine;
using NUnit.Framework;
using System.IO;

namespace InterpreterUnitTest
{
    class ReturnStatementsUnitTest
    {
        [Test]
        public void Return_NestedIfStatements_AssertEndsExecution()
        {
            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var dir = Path.Combine(rootDir, "return.abjad");

            var outputPath = Path.Combine(rootDir, "return.txt");

            var code = Reader.Read(dir);

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = "بداية فو\nقبل الارجاع\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }
    }
}
