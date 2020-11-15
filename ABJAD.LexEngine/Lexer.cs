using ABJAD.Models;
using ABJAD.Models.Exceptions;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
            { "أيضا", TokenType.AND },
            { "ايضا", TokenType.AND },
            { "صنف", TokenType.CLASS },
            { "ثابت", TokenType.CONST },
            { "غيره", TokenType.ELSE },
            { "خطأ", TokenType.FALSE },
            { "خطا", TokenType.FALSE },
            { "بـ", TokenType.FOR },
            { "دالة", TokenType.FUNC },
            { "إذا", TokenType.IF },
            { "اذا", TokenType.IF },
            { "إنشاء", TokenType.NEW },
            { "عدم", TokenType.NULL },
            { "أو", TokenType.OR },
            { "او", TokenType.OR },
            { "أكتب", TokenType.PRINT },
            { "اكتب", TokenType.PRINT },
            { "أرجع", TokenType.RETURN },
            { "ارجع", TokenType.RETURN },
            { "صحيح", TokenType.TRUE },
            { "طالما", TokenType.WHILE },
            { "متغير", TokenType.VAR },
        };

        private readonly string code;

        private List<Token> Tokens;

        private int _line = 1;
        private int _lineIndex = 0;
        private int _current = 0;

        public Lexer(string code)
        {
            this.code = code;
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
                case '؛': AddToken(TokenType.SEMICOLON); return;
                case '،': AddToken(TokenType.COMMA); return;
                case '.': AddToken(TokenType.DOT); return;
                case '{': AddToken(TokenType.OPEN_BRACE); return;
                case '}': AddToken(TokenType.CLOSE_BRACE); return;
                case '(': AddToken(TokenType.OPEN_PAREN); return;
                case ')': AddToken(TokenType.CLOSE_PAREN); return;
                case '+': AddToken(TokenType.PLUS); return;
                case '*': AddToken(TokenType.TIMES); return;
                case '-':AddToken(TokenType.MINUS); return;
                case '"':
                    var str = new StringBuilder();
                    while (!IsNext('"', out char next))
                    {
                        str.Append(next);
                    }
                    AddToken(TokenType.STRING_CONST, str.ToString());
                    return;
                case '\\':
                    if (Match('\\'))
                    {
                        while (Peek() != '\n') continue;

                        DecrementIndex(1);
                    }
                    else
                    {
                        AddToken(TokenType.DIVIDED_BY);
                    }
                    return;
                case '=':
                    if (Match('='))
                        AddToken(TokenType.EQUAL_EQUAL);
                    else
                        AddToken(TokenType.EQUAL);
                    return;
                case '!':
                    if (Match('='))
                        AddToken(TokenType.BANG_EQUAL);
                    else
                        AddToken(TokenType.BANG);
                    return;
                case '<':
                    if (Match('='))
                        AddToken(TokenType.LESS_EQUAL);
                    else
                        AddToken(TokenType.LESS_THAN);
                    return;
                case '>':
                    if (Match('='))
                        AddToken(TokenType.GREATER_EQUAL);
                    else
                        AddToken(TokenType.GREATER_THAN);
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
                throw new AbjadExpectedTokenNotFoundException(_line, expected.ToString());
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
            Tokens.Add(new Token(type, code[_current - 1]));
        }

        private void AddToken(TokenType type, string text)
        {
            Tokens.Add(new Token(type, text));
        }

        private bool ScanLiteral()
        {
            var word = NextWord();
            if (Regex.IsMatch(word, LiteralRegex))
            {
                IncrementIndex(word.Length - 1);
                AddToken(TokenType.ID, word);
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
                AddToken(TokenType.NUMBER_CONST, word);
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
    }
}
