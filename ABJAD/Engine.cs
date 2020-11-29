using ABJAD.InterpretEngine;
using ABJAD.IO;
using ABJAD.LexEngine;
using ABJAD.Models.Exceptions;
using CommandLine;

namespace ABJAD
{
    class Engine
    {
        public class Options
        {
            [Option('o', Required = false, HelpText = "Set the ouput text file path (Defualts to the name of the ABJAD program with .txt extension.")]
            public string OutputPath { get; set; }
        }

        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    Execute(args[0], o.OutputPath);
                });
            //Execute("C:\\Users\\mezdn\\Desktop\\ABJAD\\casting.abjad", null);
        }

        static void Execute(string programPath, string outputPath)
        {
            if (outputPath == null)
            {
                outputPath = programPath.Split(".abjad")[0] + ".txt";
            }
            var writer = new Writer(outputPath);

            try
            {
                var code = Reader.Read(programPath);

                var lexer = new Lexer(code);
                var tokens = lexer.Lex();

                var parser = new ParseEngine.Parser(tokens);
                var bindings = parser.Parse();

                var interpreter = new Interpreter(writer);
                interpreter.Interpret(bindings);
            }
            catch (AbjadException e)
            {
                writer.Write(e.ArabicMessage);
            }
        }
    }
}
