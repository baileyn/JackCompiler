using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackCompiler.Hack
{

	/*
    public class CompilationException : Exception
    {
        public CompilationException(Token token, string message)
            : this(token.LineNumber, token.CharPosition, message)
        {

        }

        public CompilationException(int lineNumber, int charPosition, string message)
            : base(string.Format("{0}:{1} {2}", lineNumber, charPosition, message))
        {
        }
    }

    public class Compiler
    {
        private Program m_HackProgram;
        private List<Assembly> m_Assemblies = new List<Assembly>();

        public string OutputFile
        {
            get
            {
                var fileName = Path.GetFileName(m_HackProgram.ClassPath);
                return Path.Combine(m_HackProgram.ClassPath, fileName + ".asm");
            }
        }
        
        public IReadOnlyList<Assembly> Assemblies
        {
            get
            {
                return m_Assemblies.AsReadOnly();
            }
        }

        public Compiler(Program program)
        {
            m_HackProgram = program;
        }

        public void Compile()
        {
            foreach(var file in m_HackProgram.Files)
            {
                var assembly = Compile(file);
                m_Assemblies.Add(assembly);
            }
        }
        public void Write()
        {
            var instruction = CallInstruction.GenerateCallInstruction("Sys.init");

            StringBuilder assemblerCode = new StringBuilder();
            // Add the bootstrap code to the beginning.
            assemblerCode.AppendFormat(@"@256
D=A
@SP
M=D

{0}
", instruction.GenerateAssembly());


            foreach(var assembly in m_Assemblies)
            {
                assemblerCode.AppendLine("// Assembly for file " + assembly.ClassName + ".vm");
                assemblerCode.AppendLine(assembly.Generate());
            }

            File.WriteAllText(OutputFile, assemblerCode.ToString());
        }

        public static Instruction Compile(string className, TokenSequence tokenSequence)
        {
            var tokens = tokenSequence.Tokens;

            var firstToken = tokens.FirstOrDefault();

            if(firstToken == null)
            {
                throw new InvalidOperationException("Token sequence doesn't contain any tokens.");
            }
            
            if(firstToken.Type != TokenType.Instruction)
            {
                throw new CompilationException(firstToken, "Expected an Instruction, but found a " + firstToken.Type);
            }


            AssemblyCompilers.AssemblyCompiler compiler;
            
            if(!AssemblyCompilers.Compilers.TryGetValue(firstToken.Data.ToLower(), out compiler))
            {
                throw new CompilationException(firstToken, "Unhandled instruction: " + firstToken.Data);
            }
            
            return compiler(className, tokenSequence);
        }

        public static Assembly Compile(string className, Parser parser)
        {
            var builder = new AssemblyBuilder(className);

            foreach(TokenSequence sequence in parser.Parse())
            {
                var instruction = Compile(className, sequence);
                builder.AppendInstruction(instruction);
            }

            return builder.ToAssembly();
        }

        public static Assembly Compile(string file)
        {
            if(!File.Exists(file))
            {
                throw new ArgumentException("File doesn't exist: " + file);
            }
            
            Lexer lexer = new Lexer(File.ReadAllText(file));
            Parser parser = new Parser(lexer);

            var className = Path.GetFileNameWithoutExtension(file);

            return Compile(className, parser);
        }
    }*/
}
