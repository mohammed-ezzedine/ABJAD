using ABJAD.InterpretEngine;
using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.ParseEngine;
using NUnit.Framework;

namespace InterpreterUnitTest
{
    class ClassesUnitTest
    {
        [Test]
        public void Classes_MultipleInstances_NoErrors()
        {
            var code =
                @"صنف اختبار {
  متغير أول = 0؛
  متغير ثاني = عدم؛
  
  دالة اختبار() {
    ثاني = 2؛
  }

  دالة تحصيل_ثاني() {
    أرجع ثاني؛
  }
}

متغير أ = انشئ إختبار()؛
متغير ب = انشئ اختبار()؛
أكتب (أ.تحصيل_ثاني())؛
أكتب (ب.تحصيل_ثاني())؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(null, null, true);
            var interpreter = new Interpreter(writer);

            Assert.DoesNotThrow(new TestDelegate(() =>
            {
                interpreter.Interpret(bindings);
            }));
        }

        [Test]
        public void Classes_MultipleInstances_AreEqual()
        {
            var code =
                @"صنف اختبار {
  متغير أول = 0؛
  متغير ثاني = عدم؛
  
  دالة اختبار(أ) {
    ثاني = أ؛
  }

  
  دالة تحصيل_ثاني() {
    أرجع ثاني؛
  }
}

متغير أ = انشئ إختبار(2)؛
متغير ب = انشئ اختبار(4)؛
أكتب (أ.تحصيل_ثاني())؛
أكتب (ب.تحصيل_ثاني())؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(null, null, true);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = "2\n4";
            var output = writer.GetOutput();
            Assert.AreEqual(expectedOutput, output);
        }

        [Test]
        public void Classes_MultipleConstructors_AreEqual()
        {
            var code =
                @"صنف اختبار {
  متغير أول = 0؛
  متغير ثاني = عدم؛
  دالة اختبار() {
    ثاني = 2؛
  }

  دالة اختبار(أ) {
    ثاني = ا؛
  }

  دالة تحصيل_ثاني() {
    أرجع ثاني؛
  }
}

متغير أ = انشئ إختبار()؛
متغير ب = انشئ اختبار(3)؛
أكتب (أ.تحصيل_ثاني())؛
أكتب (ب.تحصيل_ثاني())؛";

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new Parser(tokens);
            var bindings = parser.Parse();

            var writer = new FileWriter(null, null, true);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);

            var expectedOutput = "2\n3";
            var output = writer.GetOutput();
            Assert.AreEqual(expectedOutput, output);
        }
    }
}
