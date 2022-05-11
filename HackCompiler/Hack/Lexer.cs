using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HackCompiler.Hack
{
    public enum State
    {
		Base,
		Integer,
		String,
		Text,
		LineComment,
		MultiLineComment,
    }

    public enum TokenType
    {
        Keyword,
		Symbol,
		Integer,
		String,
		Identifier
    }

    public class Token
    {
        public TokenType Type
        {
            get;
            private set;
        }

        public int LineNumber
        {
            get;
            private set;
        }

        public int CharPosition
        {
            get;
            private set;
        }

        public string Data
        {
            get;
            private set;
        }

        public Token(TokenType type, int lineNumber, int charPosition, string data)
        {
            Type = type;
            LineNumber = lineNumber;
            CharPosition = charPosition;
            Data = data;
        }
    }



    public class Lexer
    {
        private List<Token> m_Tokens = new List<Token>();

        /// <summary>
        /// the entire contents of the Virtual Machine File that is to be lexically tokenized
        /// </summary>
        private string m_Line;

        private int m_LineIndex = 0;

        /// <summary>
        /// the line position in the m_Lines array
        /// </summary>
        private int m_LineNumber = 1;
        
        /// <summary>
        /// the character position in the line pointed to by the <code>LineIndex</code>
        /// </summary>
        private int m_CharPosition = 1;

        /// <summary>
        /// the <code>StringBuilder</code> used to build the <code>Token</code>'s data
        /// </summary>
        private StringBuilder m_Data = new StringBuilder();

        /// <summary>
        /// the current <code>State</code> of the <code>Lexer</code>
        /// </summary>
        private State m_CurrentState = State.Base;

        public IEnumerable<Token> Tokens
        {
            get
            {
                return m_Tokens.AsReadOnly();
            }
        }

        private List<char> validCharacters = new List<char> { '_', };

        private List<string> keywords = new List<string> {
            "class", "constructor", "function", "method", "field",
			"static", "var", "int", "char", "boolean", "void", "true",
			"false", "null", "this", "let", "do", "if", "else", "while",
			"return"
        };

        private List<char> symbols = new List<char> {
			'{', '}', '(', ')', '[', ']', '.', ',', ';', '+', '-',
			'*', '/', '&', '|', '<', '>', '=', '~'
        };

        public Lexer(string line)
        {
            m_Line = line;
        }

        public void Parse()
        {
            while(m_LineIndex < m_Line.Length)
            {
                char current = PeekChar();
                char next = PeekChar(1);

                switch(m_CurrentState)
                {
                    case State.Base:
                        BaseLexer(current, next);
                        break;
					case State.LineComment:
						LineCommentLexer(current, next);
						break;
					case State.MultiLineComment:
						MultiLineCommentLexer(current, next);
						break;
					case State.String:
						StringLexer(current, next);
						break;
					case State.Text:
						TextLexer (current, next);
						break;
					case State.Integer:
						NumberLexer(current, next);
						break;
                    default:
                        throw new InvalidOperationException("Invalid state: " + m_CurrentState);
                }
            }
        }

		public void dump(string fileName) {
			var settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = "    "; // 4 spaces
			settings.NewLineOnAttributes = true;


			using(var writer = XmlWriter.Create(fileName, settings)) {
				writer.WriteStartDocument();
				writer.WriteStartElement("tokens");

				foreach (var token in Tokens) {
					string type;

					switch(token.Type) {
					case TokenType.String:
						type = "stringConstant";
						break;
					case TokenType.Integer:
						type = "integerConstant";
						break;
					default:
						type = token.Type.ToString().ToLower();
						break;
					}
						
					writer.WriteElementString(type, string.Format(" {0} ", token.Data));
				}

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
		}

        private void Reject()
        {
            // Grab the current character and proceed to the next index,
            // but don't worry about storing it.
            NextChar();
        }

        private void Accept()
        {
            // Grab the current character and proceed to the next index.
            char c = NextChar();

            m_Data.Append(c);
        }

        private void EmitToken(TokenType type)
        {
            m_Tokens.Add(new Token(type, m_LineNumber, m_CharPosition, m_Data.ToString()));
            m_Data.Clear();
        }

        private char PeekChar(int lookAhead = 0)
        {
            if(m_LineIndex + lookAhead >= m_Line.Length)
            {
                return Char.MinValue;
            }

            return m_Line[m_LineIndex + lookAhead];
        }

        /// <summary>
        /// Increases the index of the string being processed. If the current character
        /// is a newline character, the <code>Line Number</code> is increased by one and
        /// the <code>Character Position</code> is set to zero. Otherwise, the <code>
        /// Character Position</code> is just increased by one.
        /// </summary>
        /// <returns>the character that was just processed</returns>
        private char NextChar()
        {
            if(m_LineIndex >= m_Line.Length)
            {
                return Char.MinValue;
            }

            char currentChar = m_Line[m_LineIndex];
            m_LineIndex++;

            if(currentChar == '\n')
            {
                m_LineNumber++;
                m_CharPosition = 0;
            }

            m_CharPosition++;

            return currentChar;
        }

        /// <summary>
        /// The base state is in charge of discarding all whitespace, and 
        /// setting the appropriate state needed.
        /// </summary>
        private void BaseLexer(char current, char next)
        {
			if(Char.IsWhiteSpace(current)) {
				Reject();
			} else if(Char.IsLetter(current) || validCharacters.Contains(current)) {
				m_CurrentState = State.Text;
			} else if(current == '/' && next == '/') {
				m_CurrentState = State.LineComment;
			} else if(Char.IsDigit(current)) {
				m_CurrentState = State.Integer;
			} else if(current == '\"') {
				Reject();
				m_CurrentState = State.String;
			} else if(current == '/' && next == '*') {
				Reject();
				Reject();
				m_CurrentState = State.MultiLineComment;
			} else if(symbols.Contains(current)) {
				Accept();
				EmitToken(TokenType.Symbol);
			} 
        }

        private void TextLexer(char current, char next)
        {
            if(Char.IsLetterOrDigit(current) || validCharacters.Contains(current))
            {
                Accept();
            }
            else
            {
				if(keywords.Contains(m_Data.ToString()))
                {
                    EmitToken(TokenType.Keyword);
                    m_CurrentState = State.Base;
                }
                else
                {
					EmitToken(TokenType.Identifier);
                    m_CurrentState = State.Base;
                }
            }

            // If we're at the end of the stream.
            if(next == Char.MinValue)
			{
				if(keywords.Contains(m_Data.ToString()))
				{
					EmitToken(TokenType.Keyword);
					m_CurrentState = State.Base;
				}
				else
				{
					EmitToken(TokenType.Identifier);
					m_CurrentState = State.Base;
				}
            }
        }

		private void StringLexer(char current, char next) 
		{
			if(current == '\"') {
				Reject();
				EmitToken(TokenType.String);
				m_CurrentState = State.Base;
			} else {
				Accept();
			}
		}

        private void LineCommentLexer(char current, char next)
        {
            Reject();

            if (next == '\n')
            {
                m_CurrentState = State.Base;
            }
        }

		private void MultiLineCommentLexer(char current, char next) 
		{
			if (current == '*' && next == '/') {
				Reject ();
				Reject ();
				m_CurrentState = State.Base;
			} else {
				Reject ();
			}
		}

        private void NumberLexer(char current, char next)
        {
            if(Char.IsDigit(current))
            {
                Accept();
            }

            if(!Char.IsDigit(next))
            {
                EmitToken(TokenType.Integer);
                m_CurrentState = State.Base;
            }
        }
    }
}
