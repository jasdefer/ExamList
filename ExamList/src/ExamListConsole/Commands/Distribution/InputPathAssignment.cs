using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListConsole.Commands.Distribution
{
    public class InputPathAssignment : IAssignment
    {
        private readonly string[] _Keys = new string[] { "input", "in" };
        private bool _Valid = true;
        public string Path { get; private set; }
        public string Description
        {
            get
            {
                return "Set the path of the csv file with the information about the students";
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
            if (!File.Exists(Path))
            {
                Console.WriteLine("Invalid path");
                _Valid = false;
            }
        }

        public void Reassign()
        {
            Console.WriteLine($"Inputpath not set: {Description}");
            do
            {
                Console.WriteLine("Please enter a valid path:");
                Path = Console.ReadLine();
            } while (!File.Exists(Path));
        }
    }
}
