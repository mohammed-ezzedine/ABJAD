using ABJAD.InterpretEngine;
using ABJAD.IO;
using ABJAD.LexEngine;
using CommandLine;

namespace ABJAD
{
    class Engine
    {
        public class Options
        {
            [Option('i', Required = true, HelpText = "Provide the path of the ABJAD program file.")]
            public string ProgramPath { get; set; }

            [Option('o', Required = false, HelpText = "Set the ouput text file path (Defualts to the name of the ABJAD program with .txt extension.")]
            public string OutputPath { get; set; }
        }

        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    Execute(o.ProgramPath, o.OutputPath);
                });
        }

        static void Execute(string programPath, string outputPath)
        {
            if (outputPath == null)
            {
                outputPath = programPath.Split(".abjad")[0] + ".txt";
            }

            var code = Reader.Read(programPath);

            var lexer = new Lexer(code);
            var tokens = lexer.Lex();

            var parser = new ParseEngine.Parser(tokens);
            var bindings = parser.Parse();

            var writer = new Writer(outputPath);
            var interpreter = new Interpreter(writer);
            interpreter.Interpret(bindings);
        }
    }
}
