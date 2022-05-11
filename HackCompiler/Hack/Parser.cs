using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack
{
    public class Parser
    {
        private Lexer m_Lexer;

        public Parser(Lexer lexer)
        {
            m_Lexer = lexer;
            m_Lexer.Parse();
        }

		public void Parse() {
			
		}

		private void parseShit() {
		}
    }
}
