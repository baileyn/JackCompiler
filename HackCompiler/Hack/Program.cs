using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HackCompiler.Hack
{
    public class Program
    {
        public const string VirtualMachineFileSuffix = ".vm";

        private string m_ClassPath;
        private List<string> m_Files = new List<string>();

        public string ClassPath
        {
            get
            {
                return m_ClassPath;
            }
        }

        public IReadOnlyList<string> Files
        {
            get
            {
                return m_Files.AsReadOnly();
            }
        }

        /// <summary>
        /// Create an application where the classes are in the specified folder
        /// </summary>
        /// <param name="classpath">the folder where the application's files are located</param>
        public Program(string classpath)
        {
            if(!Directory.Exists(classpath))
            {
                throw new ArgumentException("Invalid Directory: " + classpath);
            }
            
            m_ClassPath = classpath;

            LoadProgramFiles();
        }

        /// <summary>
        /// Loads all of the files in the <code>ClassPath</code> directory that end with the <code>VirtualMachineFileSuffix</code>
        /// file suffix. 
        /// </summary>
        private void LoadProgramFiles()
        {
            var files = from file in Directory.EnumerateFiles(m_ClassPath)
                        where file.EndsWith(VirtualMachineFileSuffix)
                        select file;

            m_Files.AddRange(files);
        }
    }
}
