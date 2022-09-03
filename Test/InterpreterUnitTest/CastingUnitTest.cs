using ABJAD.InterpretEngine;
using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.Models;
using ABJAD.ParseEngine;
using NUnit.Framework;
using System.IO;

namespace InterpreterUnitTest
{
    class CastingUnitTest
    {
        [Test]
        public void CastingOperations_FromNumberToStr_AreEqual()
        {
            var code = 
                @"متغير رقمي = 3؛
متغير رقمي_مقطع = مقطع(رقمي)؛
أكتب(رقمي_مقطع == ""3"")؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var outputPath = Path.Combine(rootDir, "casting1.txt");

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = Constants.Boolean.True + "\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void CastingOperations_FromBoolToStr_AreEqual()
        {
            var code =
                @"متغير رقمي = 3؛متغير شرطي = خطأ؛
متغير شرطي_مقطع = مقطع(شرطي)؛
أكتب(شرطي_مقطع == ""خطأ"")؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var outputPath = Path.Combine(rootDir, "casting2.txt");

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = Constants.Boolean.True + "\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void CastingOperations_FromStrToBool_AreEqual()
        {
            var code =
                @"متغير شرط_مقطعي = ""صحيح""؛
أكتب(منطق(شرط_مقطعي) == صحيح)؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var outputPath = Path.Combine(rootDir, "casting3.txt");

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = Constants.Boolean.True + "\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void CastingOperations_TypeofBool_AreEqual()
        {
            var code =
                @"متغير شرطي = خطأ؛
أكتب(نوع(شرطي))؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var outputPath = Path.Combine(rootDir, "casting4.txt");

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = Constants.Types.Bool + "\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void CastingOperations_TypeofStr_AreEqual()
        {
            var code =
                @"متغير مقطعي = ""مرحبا، بالعالم""؛
أكتب(نوع(مقطعي))؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var outputPath = Path.Combine(rootDir, "casting5.txt");

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = Constants.Types.String + "\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void CastingOperations_TypeofNumber_AreEqual()
        {
            var code =
                @"متغير رقمي = 3؛
أكتب(نوع(رقمي))؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var outputPath = Path.Combine(rootDir, "casting6.txt");

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = Constants.Types.Number + "\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void CastingOperations_TypeofCustom_AreEqual()
        {
            var code =
                @"صنف انسان {
	دالة انسان() {}
}
متغير محمد = انشئ انسان()؛
اكتب(نوع(محمد) == ""انسان"")؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var binDir = Directory.GetCurrentDirectory();
            var rootDir = Directory.GetParent(binDir).Parent.Parent.Parent.FullName;
            var outputPath = Path.Combine(rootDir, "casting7.txt");

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = Constants.Boolean.True + "\n";
            var output = File.ReadAllText(outputPath);
            Assert.AreEqual(expectedOutput, output);
        }
    }
}
