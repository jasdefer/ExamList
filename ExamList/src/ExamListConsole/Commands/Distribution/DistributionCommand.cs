using ExamListConsole.Helper;
using ExamListCore;
using ExamListCore.CsvReader;
using ExamListCore.CsvReader.StudentReaders;
using ExamListCore.Latex;
using ExamListCore.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListConsole.Commands.Distribution
{
    public class DistributionCommand : ICommand
    {
        public List<IArgument> Arguments { get; }
        private InputPathAssignment _InputPath = new InputPathAssignment();
        private OutputPathAssignment _OutputPath = new OutputPathAssignment();
        private RoomAmountAssignment _RoomAmount = new RoomAmountAssignment();

        public DistributionCommand()
        {
            Arguments = new List<IArgument>();
            Arguments.Add(_InputPath);
            Arguments.Add(_OutputPath);
            Arguments.Add(_RoomAmount);
        }

        public void Run(string[] args)
        {
            if (string.IsNullOrEmpty(_InputPath.Path))
            {
                _InputPath.Reassign();
            }

            if (string.IsNullOrEmpty(_OutputPath.Path))
            {
                _OutputPath.Reassign();
            }

            if (_RoomAmount.Amount == 0)
            {
                _RoomAmount.Reassign();
            }

            Distributor distributor = SetRooms();

            Start(distributor);
        }

        private void Start(Distributor distributor)
        {
            StineExtendedList options = new StineExtendedList();
            options.ThrowExceptionOnInvalidStudent = true;
            //Get the students from the csv
            List<Student> students = null;
            try
            {
                students = CsvReader.ReadStudents(_InputPath.Path, '\t', options);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid file");
                return;
            }
            distributor.Students = students;
            try
            {
                distributor.Distribute();
            }
            catch(InvalidOperationException)
            {
                Console.WriteLine("Room capacity is not enough");
                return;
            }
            //Get the latex syntax and save it to the file
            string result = ExamConverter.ForExam(students);
            try
            {
                System.IO.File.WriteAllText(_OutputPath.Path, result);
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot write to file");
            }

            Console.WriteLine("End of command");
        }

        private Distributor SetRooms()
        {
            //Ask the user for the rooms
            Distributor distributor = new Distributor();
            for (int i = 0; i < _RoomAmount.Amount; i++)
            {
                Console.WriteLine($"Enter the name for room ({i + 1}):");
                string name = Console.ReadLine();
                int capacity = InputHelper.AskInt("Enter a valid capacity", 1, 1000);
                //If the user is not sure, redo the room
                if (!InputHelper.AreYouSure($"Name {name}, capacity={capacity}"))
                {
                    i--;
                }
                else
                {
                    distributor.AddRoom(name, capacity);
                }
            }
            return distributor;
        }
    }
}
