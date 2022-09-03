﻿using ABJAD.InterpretEngine;
using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.ParseEngine;
using NUnit.Framework;
using System.IO;

namespace InterpreterUnitTest
{
    class OperatorsUnitTest
    {
        [Test]
        public void Interpreting_Operators_Output()
        {
            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var dir = Path.Combine(rootDir, "test9.abjad");

            var outputPath = Path.Combine(rootDir, "test9.txt");

            var code = Reader.Read(dir);

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = "خطا\nخطا\nخطا\nخطا\nصحيح\nمرحبا بالعالم\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void Interpreting_StringOperators_Output()
        {
            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var dir = Path.Combine(rootDir, "string-operations.abjad");

            var outputPath = Path.Combine(rootDir, "string-operations.txt");

            var code = Reader.Read(dir);

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = "صحيح\nصحيح\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }
    }
}
