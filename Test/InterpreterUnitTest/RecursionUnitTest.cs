using ABJAD.InterpretEngine;
using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.ParseEngine;
using NUnit.Framework;
using System.IO;

namespace InterpreterUnitTest
{
    class RecursionUnitTest
    {
        [Test]
        public void Interpreting_Recursion_2Power9()
        {
            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var dir = Path.Combine(rootDir, "recursive-power.abjad");

            var outputPath = Path.Combine(rootDir, "recursive-power.txt");

            var code = Reader.Read(dir);

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = "512\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void Interpreting_Factorial_FactoOf10()
        {
            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var dir = Path.Combine(rootDir, "recursive-factorial.abjad");

            var outputPath = Path.Combine(rootDir, "recursive-factorial.txt");

            var code = Reader.Read(dir);

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = "3628800\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void Interpreting_Fibonacci_FibOf10()
        {
            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var dir = Path.Combine(rootDir, "recursive-fibonacci.abjad");

            var outputPath = Path.Combine(rootDir, "recursive-fibonacci.txt");

            var code = Reader.Read(dir);

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = "55\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }
    }
}
