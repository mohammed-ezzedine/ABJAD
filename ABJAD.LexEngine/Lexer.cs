using ABJAD.Models;
using ABJAD.Models.Exceptions;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static ABJAD.Models.TokenType;

namespace ABJAD.LexEngine
{
    public class Lexer
    {
        private static readonly string DigitRegex = "[0-9]";
        private static readonly string NumberRegex = @"^(0|[1-9][0-9]*)(\.[0-9]*)?$";
        private static readonly string LetterRegex = $"[\u0620-\u063A]|[\u0641-\u064A]";
        private static readonly string LiteralRegex = @$"({LetterRegex})({LetterRegex}|{DigitRegex}|(_))*";
        private static readonly string WordTerminalRegex = @"[();؛×،{} !@#$%&*-+=.,/\`~'"":\\\[\]\?\^]";
        private static readonly string NumberTerminalRegex = @"[();؛×،{} !@#$%&*-+=/\`~'"":\\\[\]\?\^]";

        private static readonly Dictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>
        {
            { "منطق", BOOL },
            { "صنف", CLASS },
            { "ثابت", CONST },
            { "والا", ELSE },
            { "خطا", FALSE },
            { "كرر", FOR },
            { "دالة", FUNC },
            { "اذا", IF },
            { "انشئ", NEW },
            { "عدم", NULL },
            { "رقم", NUMBER },
            { "اكتب", PRINT },
            { "ارجع", RETURN },
            { "مقطع", STRING },
            { "صحيح", TRUE },
            { "نوع", TYPEOF },
            { "طالما", WHILE },
            { "متغير", VAR },
        };

        private readonly string code;

        private List<Token> Tokens;

        private int _line = 1;
        private int _lineIndex = 0;
        private int _current = 0;

        public Lexer(string code)
        {
            this.code = IgnoreCaseSensitivity(code);
        }

        public List<Token> Lex()
        {
            Tokens = new List<Token>();
            char c;
            while (HasNext(out c))
            {
                ScanToken(c);
            }

            return Tokens;
        }

        private bool HasNext(out char c)
        {
            if (_current >= code.Length)
            {
                c = default;
                return false;
            }

            c = code[_current];
            IncrementIndex(1);
            return true;
        }

        private void ScanToken(char c)
        {
            switch (c)
            {
                case '\t': return;
                case '\r': return;
                case ' ': return; // skip white spaces
                case '\n': _line++; _lineIndex = 0; return;
                case '؛': AddToken(SEMICOLON); return;
                case '،': AddToken(COMMA); return;
                case '.': AddToken(DOT); return;
                case '{': AddToken(OPEN_BRACE); return;
                case '}': AddToken(CLOSE_BRACE); return;
                case '(': AddToken(OPEN_PAREN); return;
                case ')': AddToken(CLOSE_PAREN); return;
                case '+':
                    if (Match('+'))
                    {
                        AddToken(EQUAL);
                        AddToken(Previous(1));
                        AddToken(PLUS);
                        AddToken(NUMBER_CONST, "1");
                    }
                    else if (Match('='))
                    {
                        AddToken(EQUAL);
                        AddToken(Previous(1));
                        AddToken(PLUS);
                    }
                    else
                    {
                        AddToken(PLUS);
                    }
                    return;
                case '-':
                    if (Match('-'))
                    {
                        AddToken(EQUAL);
                        AddToken(Previous(1));
                        AddToken(MINUS);
                        AddToken(NUMBER_CONST, "1");
                    }
                    else if (Match('='))
                    {
                        AddToken(EQUAL);
                        AddToken(Previous(1));
                        AddToken(MINUS);
                    }
                    else
                    {
                        AddToken(MINUS);
                    }
                    return;
                case '*':
                    if (Match('='))
                    {
                        AddToken(EQUAL);
                        AddToken(Previous(1));
                        AddToken(TIMES);
                    }
                    else
                    {
                        AddToken(TIMES);
                    }
                    return;
                case '"':
                    var str = new StringBuilder();
                    while (!IsNext('"', out char next))
                    {
                        str.Append(next);
                    }
                    AddToken(STRING_CONST, str.ToString());
                    return;
                case '\\':
                    if (Match('='))
                    {
                        AddToken(EQUAL);
                        AddToken(Previous(1));
                        AddToken(DIVIDED_BY);
                    }
                    else
                    {
                        AddToken(DIVIDED_BY);
                    }
                    return;
                case '#':
                    while (Peek() != '\n') continue; // ignore comments

                    DecrementIndex(1);
                    return;
                case '=':
                    if (Match('='))
                        AddToken(EQUAL_EQUAL);
                    else
                        AddToken(EQUAL);
                    return;
                case '!':
                    if (Match('='))
                        AddToken(BANG_EQUAL);
                    else
                        AddToken(BANG);
                    return;
                case '<':
                    if (Match('='))
                        AddToken(LESS_EQUAL);
                    else
                        AddToken(LESS_THAN);
                    return;
                case '>':
                    if (Match('='))
                        AddToken(GREATER_EQUAL);
                    else
                        AddToken(GREATER_THAN);
                    return;
                case '&':
                    if (Match('&'))
                        AddToken(AND);
                    else
                        throw new AbjadUnexpectedTokenException(_line, _lineIndex, Peek().ToString());
                    return;
                case '|':
                    if (Match('|'))
                        AddToken(OR);
                    else
                        throw new AbjadUnexpectedTokenException(_line, _lineIndex, Peek().ToString());
                    return;
                default:
                    if (ScanKeyword()) return;
                    if (ScanNumber()) return;
                    if (ScanLiteral()) return;
                    throw new AbjadUndefinedTokenException(_line, _lineIndex, c.ToString());
            }
        }

        private string NextWord()
        {
            int currentCopy = _current - 1;
            char currentChar;
            var wordBuilder = new StringBuilder();

            while (!Regex.IsMatch((currentChar = code[currentCopy++]).ToString(), WordTerminalRegex))
            {
                wordBuilder.Append(currentChar);
            }

            return wordBuilder.ToString();
        }

        private string NextNumber()
        {
            int currentCopy = _current - 1;
            char currentChar;
            var wordBuilder = new StringBuilder();

            while (!Regex.IsMatch((currentChar = code[currentCopy++]).ToString(), NumberTerminalRegex))
            {
                wordBuilder.Append(currentChar);
            }

            return wordBuilder.ToString();
        }

        private bool IsNext(char expected, out char next)
        {
            if (!HasNext(out next))
            {
                throw new AbjadExpectedTokenNotFoundException(expected.ToString(), _line, _lineIndex);
            }

            if (next == expected)
            {
                return true;
            }

            return false;
        }

        private void IncrementIndex(int num)
        {
            _current += num;
            _lineIndex += num;
        }

        private void DecrementIndex(int num)
        {
            _current -= num;
            _lineIndex -= num;
        }

        private void AddToken(TokenType type)
        {
            Tokens.Add(new Token(type, code[_current - 1], _line, _lineIndex));
        }

        private void AddToken(TokenType type, string text)
        {
            Tokens.Add(new Token(type, text, _line, _lineIndex));
        }

        private void AddToken(Token token)
        {
            var newToken = new Token(token.Type, token.Text, token.Line, token.Index);
            Tokens.Add(newToken);
        }

        private string IgnoreCaseSensitivity(string str)
        {
            return str
                .Replace("ـ", "")
                .Replace('آ', 'ا')
                .Replace('أ', 'ا')
                .Replace('إ', 'ا');
        }

        private bool ScanLiteral()
        {
            var word = NextWord();
            if (Regex.IsMatch(word, LiteralRegex))
            {
                IncrementIndex(word.Length - 1);
                AddToken(ID, word);
                return true;
            }

            return false;
        }

        private bool ScanNumber()
        {
            var word = NextNumber();

            if (word.Length > 0 && Regex.IsMatch(word, NumberRegex))
            {
                IncrementIndex(word.Length - 1);
                AddToken(NUMBER_CONST, word);
                return true;
            }

            return false;
        }

        private bool ScanKeyword()
        {
            return MatchKeyword(NextWord());
        }

        private bool MatchKeyword(string word)
        {
            if (Keywords.ContainsKey(word))
            {
                IncrementIndex(word.Length - 1);
                AddToken(Keywords[word], word);
                return true;
            }

            return false;
        }

        private bool Match(char expected)
        {
            if (HasNext(out char c))
            {
                if (c == expected)
                {
                    return true;
                }

                DecrementIndex(1);
            }

            return false;
        }

        private bool Match(string regex)
        {
            if (HasNext(out char c))
            {
                if (Regex.IsMatch(c.ToString(), regex))
                {
                    return true;
                }

                DecrementIndex(1);
            }

            return false;
        }

        private char Peek()
        {
            if (_current >= code.Length)
            {
                _current = int.MaxValue;
                return '\n';
            }

            var c = code[_current];
            IncrementIndex(1);
            return c;
        }

        private Token Previous(int i = 0)
        {
            var length = Tokens.Count;
            return Tokens[length - i - 1];
        }
    }
}
