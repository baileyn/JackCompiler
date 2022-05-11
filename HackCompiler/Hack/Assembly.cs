using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HackCompiler.Hack
{
    public class Assembly
    {
        private List<Instruction> m_Instructions = new List<Instruction>();
        private string m_ClassName;

        /// <summary>
        /// The name of the class represented by this Assembly
        /// </summary>
        public string ClassName
        {
            get
            {
                return m_ClassName;
            }
        }

        /// <summary>
        /// The Instructions that make up this <code>Assembly</code>
        /// </summary>
        public IReadOnlyList<Instruction> Instructions
        {
            get
            {
                return m_Instructions.AsReadOnly();
            }
        }

        /// <summary>
        /// Constructs an <code>Assembly</code> with all of the specified instructions
        /// </summary>
        /// <param name="instructions">the instructions to supply to the <code>Assembly</code></param>
        public Assembly(string className, IEnumerable<Instruction> instructions)
        {
            m_ClassName = className;
            m_Instructions.AddRange(instructions);
        }
        
        /// <summary>
        /// Generates the assembly instructions for the higher-level <code>Virtual Machine</code> instructions.
        /// </summary>
        /// <returns>the generated assembly</returns>
        public string Generate()
        {
            StringBuilder generatedAssembly = new StringBuilder();

            foreach (var instruction in m_Instructions)
            {
                generatedAssembly.AppendFormat("// {0}\n", instruction.Raw);
                generatedAssembly.AppendLine(instruction.GenerateAssembly());
            }

            return generatedAssembly.ToString();
        }
    }
}
