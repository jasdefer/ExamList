using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListConsole.Commands.Distribution
{
    public class OutputPathAssignment : IAssignment
    {
        private readonly string[] _Keys = new string[] { "output", "out" };
        private bool _Valid = true;
        public string Path { get; private set; }
        public string Description
        {
            get
            {
                return "Set the directory, where the output files are stored";
            }
        }

        public string[] Keys
        {
            get
            {
                return _Keys;
            }
        }

        public bool Valid
        {
            get
            {
                return _Valid;
            }
        }

        public void Assign(string argument)
        {
            Path = argument;
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(Path)))
            {
                Console.WriteLine("Invalid directory");
                _Valid = false;
            }
        }

        public void Reassign()
        {
            Console.WriteLine($"Outputpath not set: {Description}");
            do
            {
                Console.WriteLine("Please enter a valid directory:");
                Path = Console.ReadLine();
            } while (!Directory.Exists(System.IO.Path.GetDirectoryName(Path)));
        }
    }
}
