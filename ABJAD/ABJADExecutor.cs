using ABJAD.InterpretEngine;
using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.Models.Exceptions;
using ABJAD.ParseEngine;

namespace ABJAD
{
    public static class ABJADExecutor
    {
        public static string Execute(string code)
        {
            var writer = new BufferWriter();
            try
            {
                var lexer = new Lexer(code);
                var tokens = lexer.Lex();

                var parser = new Parser(tokens);
                var bindings = parser.Parse();

                var interpreter = new Interpreter(writer);
                interpreter.Interpret(bindings);
            }
            catch (AbjadException e)
            {
                writer.Write(e.ArabicMessage);
            }

            return writer.GetOutput();
        }
    }
}
