using ConsoleCommandHelperCore.Commands;
using ExamListCore;
using ExamListCore.CsvReader;
using ExamListCore.Latex;
using ExamListCore.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCommandHelperCore;

namespace ExamListConsole
{
    public class CreateLatexOutput : ICommand
    {
        public List<Assignment> Assignments { get; set; }
        private char _Separator = '\t';
        private bool _ReadFromDefaultExamList = true;
        private bool _ExtendStudents = false;
        private bool _ExtendStudentsFromDefault = true;
        private string _ExamPath;
        private string _ExtendStudentsPath;
        private string _BonusPointPath;
        private string _OutputPath;
        private int _Seed;
        private int _RoomAmount;
        private Dictionary<decimal, decimal> _BonusPointLevels;
        private List<Room> _Rooms;

        public string Description { get { return "Read the list of students enrolled for an exam and print the LaTeX file used for the LaTeX exam template."; } }

        public List<Statement> Statements { get; set; }

        public CreateLatexOutput()
        {
            Assignments = new List<Assignment>();
            Statements = new List<Statement>();
            Statements.Add(new Statement("examdetail",SetDetailedExamList, "Read the students from the detailed exam list, rather than the default exam list"));
            Statements.Add(new Statement("ex", ExtendStudents, "Extend the list of students with information from the course list"));
            Statements.Add(new Statement("extenddetail", ExtendFromDetailed, "for extending the student information use the detailed instead of the default course list."));

            Assignments.Add(new Assignment("separator", SetSeparator, "Set the separator of all input and output files."));
            Assignments.Add(new Assignment("exampath", SetExamListPath, "Enter the path to the list of students enrolled for the exam", true));
            Assignments.Add(new Assignment("extendpath", SetExtendPath, "Enter the path to course file."));
            Assignments.Add(new Assignment("bonuspath", SetBonusPath, "Enter the path to the bonus points"));
            Assignments.Add(new Assignment("output", SetOutputPath, "Enter the output path", true));
            Assignments.Add(new Assignment("seed", SetSeed, "Enter a seed for the randomness used in the student distribution"));
            Assignments.Add(new Assignment("room", SetRoomAmount, "Enter the amount of rooms used for the exam", true));
            Assignments.Add(new Assignment("bonus", SetBonus, "Enter the maximum amount of bonus points to add them to the students."));
        }


        #region Assignments
        private Status SetBonus(string arg)
        {
            //Ask for the maximum amount of achievable bonus points
            decimal bonusPoints = 0;
            bool success = decimal.TryParse(arg, out bonusPoints);
            if (!success || bonusPoints < 1 || bonusPoints > 1000)
            {
                return Status.CreateFailure("Invalid amount of maximum bonus points");
            }

            int steps = 0;
            //Ask about how many steps should the bonus points are achievable
            do
            {
                do
                {
                    //Ask for the step
                    Console.WriteLine($"How many steps between 0 and {bonusPoints} (Amount of possibles outcomes for bonus points)?");
                    string stepString = Console.ReadLine();

                    //Validate the input
                    success = int.TryParse(stepString, out steps);
                    if (!success || steps < 2 || steps > 1000)
                    {
                        Console.WriteLine("Invalid step amount");
                        success = false;
                    }
                } while (!success);

                //Calculate the bonus point levels
                success = false;
                _BonusPointLevels = new Dictionary<decimal, decimal>();
                _BonusPointLevels.Add(0, 0);
                for (int i = 1; i < steps + 1; i++)
                {
                    decimal percentage = i / (decimal)(steps+1);
                    percentage = Math.Round(percentage, 4);
                    decimal points = i*bonusPoints / (steps);
                    points = Math.Round(points, 4);
                    _BonusPointLevels.Add(percentage, points);
                }

                //Let the user confirm his input again
                Console.WriteLine("Following steps (percentage)/(bonus points):");
                Console.WriteLine(string.Join("; ", _BonusPointLevels.Select(x => $"{x.Key}/{x.Value}")));

                //Let the user confirm his input
                Console.WriteLine("Do you confirm this input (y)?");
                if (Console.ReadLine() == "y")
                {
                    success = true;
                }
            } while (!success);

            return Status.CreateSuccess();
        }

        private Status SetRoomAmount(string arg)
        {
            int rooms = 0;
            bool success = int.TryParse(arg, out rooms);
            if (!success || rooms < 1 || rooms > 100)
            {
                return Status.CreateFailure("Invalid room amount: " + arg);
            }
            else
            {
                _RoomAmount = rooms;
                return Status.CreateSuccess();
            }
        }

        private Status SetSeed(string arg)
        {
            int seed = 0;
            bool success = int.TryParse(arg, out seed);
            if (!success)
            {
                return Status.CreateFailure("Invalid seed: " + arg);
            }
            else
            {
                _Seed = seed;
                return Status.CreateSuccess();
            }
        }

        private Status SetOutputPath(string arg)
        {
            string directory = Path.GetDirectoryName(arg);
            string file = Path.GetFileName(arg);
            if (!Directory.Exists(directory))
            {
                return Status.CreateFailure("Output directory does not exists");
            }
            if (File.Exists(arg))
            {
                return Status.CreateFailure("Output file already exists: " + arg);
            }
            else
            {
                _OutputPath = arg;
                return Status.CreateSuccess();
            }
        }

        private Status SetBonusPath(string arg)
        {
            if (!File.Exists(arg))
            {
                return Status.CreateFailure("Bonus point file does not exists: " + arg);
            }
            else
            {
                _BonusPointPath = arg;
                return Status.CreateSuccess();
            }
        }
        private Status SetExtendPath(string arg)
        {
            if (!File.Exists(arg))
            {
                return Status.CreateFailure("Course file does not exists: " + arg);
            }
            else
            {
                _ExtendStudentsPath = arg;
                return Status.CreateSuccess();
            }
        }

        private Status SetExamListPath(string arg)
        {
            if (!File.Exists(arg))
            {
                return Status.CreateFailure("Exam list file does not exists: " + arg);
            }
            else
            {
                _ExamPath = arg;
                return Status.CreateSuccess();
            }
        }

        private Status SetSeparator(string arg)
        {
            char separator = '\t';
            bool success = char.TryParse(arg, out separator);
            
            if (!success)
            {
                return Status.CreateFailure($"Invalid separator: {arg}");
            }
            else
            {
                _Separator = char.Parse(arg);
                return Status.CreateSuccess();
            }
        }
        #endregion

        #region Statements
        private void ExtendFromDetailed()
        {
            _ExtendStudentsFromDefault = false;
        }

        private void ExtendStudents()
        {
            _ExtendStudents = true;
        }

        private void SetDetailedExamList()
        {
            _ReadFromDefaultExamList = false;
        }
        #endregion
        /// <summary>
        /// Read the students from the exam list, add information from the course list. Add bonus points and distribute the students to the rooms. Finally print the resulting LaTeX output file.
        /// </summary>
        public void Run()
        {
            List<Student> students;
            CreateRooms();
            
            //Read the students enrolled for the exam
            if(_ReadFromDefaultExamList)
            {
                students = StudentReader.FromDefaultExamList(_ExamPath, _Separator);
            }
            else
            {
                students = StudentReader.FromExtendedExamList(_ExamPath, _Separator);
            }

            //Read further information from the course files
            if (_ExtendStudents)
            {
                if (_ExtendStudentsFromDefault)
                {
                    ExtendStudent.ExtendFromDefaultList(_ExtendStudentsPath, students, _Separator);
                }
                else
                {
                    ExtendStudent.ExtendFromExtendedList(_ExtendStudentsPath, students, _Separator);
                }
            }

            //Add bonus points
            if(!string.IsNullOrEmpty(_BonusPointPath) && _BonusPointLevels == null)
            {
                throw new Exception("Please enter the bonus assignment, when providing a bonus link!");
            }
            if (_BonusPointLevels!=null)
            {
                BonusPointReader bonus = new BonusPointReader();
                bonus.BonusPointsLevels = _BonusPointLevels;
                bonus.AddBonusPoints(_BonusPointPath, students, _Separator);
            }

            //Distribute the students to the rooms
            Distributor distributor = new Distributor();
            distributor.Students = students;
            foreach (var room in _Rooms)
            {
                distributor.AddRoom(room);
            }
            distributor.Distribute(_Seed);
            


            //print the output
            string output = ExamConverter.PrintLatexStudentList(students);
            File.WriteAllText(_OutputPath, output);
        }

        private void CreateRooms()
        {
            _Rooms = new List<Room>();
            for (int i = 0; i < _RoomAmount; i++)
            {
                //Ask for the room name and capacity
                Console.WriteLine($"Please enter a room Name ({i+1}):");
                string roomName = Console.ReadLine();
                Console.WriteLine("Please enter the capacity");
                string capacityString = Console.ReadLine();

                //Validate the capacity
                int capacity = 0;
                bool success = int.TryParse(capacityString, out capacity);
                if (!success || capacity<1 || capacity >10000)
                {
                    //Repeat
                    Console.WriteLine("Invalid capacity");
                    i--;
                }
                else
                {
                    //Let the user confirm his input
                    Console.WriteLine($"{roomName} ({capacity}). Are you sure? (y)");
                    string sure = Console.ReadLine();
                    
                    if (sure == "y")
                    {
                        //Save the room
                        _Rooms.Add(new Room() { Name = roomName, Capacity = capacity });
                    }
                    else
                    {
                        //Repeat
                        i--;
                    }
                }
            }
        }
    }
}
