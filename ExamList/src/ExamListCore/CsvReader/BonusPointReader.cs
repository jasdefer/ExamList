using ExamListCore.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListCore.CsvReader
{
    /// <summary>
    /// Reads a list of bonus points and assign them to students
    /// </summary>
    public class BonusPointReader
    {
        /// <summary>
        /// The keys are the percentage steps, which must be reached to get the corresponding values as bonus points
        /// </summary>
        public Dictionary<decimal,decimal> BonusPointsLevels { get; set; }

        public BonusPointReader()
        {
            BonusPointsLevels = new Dictionary<decimal, decimal>();
        }

        /// <summary>
        /// Reads the percentages from the given csv file and converts them to bonus points. 
        /// </summary>
        /// <param name="path">The path with the percentages of the students</param>
        /// <param name="separator">The character separating the arguments in the csv file</param>
        public void AddBonusPoints(string path, List<Student> students, char separator = '\t')
        {
            AddBonusPoints(path, students, ReadPoints, separator);
        }

        private void AddBonusPoints(string path, List<Student> students, Action<Student,string[]> PointReader, char separator)
        {
            //Check if the bonus point levels are defined
            if(BonusPointsLevels == null || BonusPointsLevels.Count < 1)
            {
                throw new Exception("Define the bonus point levels before assigning them");
            }
            //Check the given list of students, if their email address is provided
            var studentsWithoutEmail = students.Where(x => string.IsNullOrEmpty(x.Email));
            if (studentsWithoutEmail.Count() < 1)
            {
                Console.WriteLine($"Cannot assign bonus points to at least {studentsWithoutEmail.Count()} students, because their email address is not available");
            }

            //Assign the bonus points to the students
            foreach (var line in File.ReadLines(path))
            {
                string[] args = line.Split(separator);
                //Find the student by email, because the first argument of the bonus point file always contains the email address
                Student student = students.SingleOrDefault(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLowerInvariant() == args[0].ToLowerInvariant());
                PointReader(student, args);
            }
        }

        /// <summary>
        /// Read the percentages of a student and convert them to bonus points
        /// </summary>
        /// <param name="args">The arguments coming from a csv file for the given student.</param>
        public void ReadPoints(Student student, string[] args)
        {
            
            //Validate the input
            if (args.Length != 2)
            {
                throw new Exception($"Cannot interprete {string.Join(";", args)} for bonus points.");
            }
            //Validate the email
            if (!args[0].Contains("@"))
            {
                throw new Exception("Invalid email in bonus points: " + args[0]);
            }

            //Check if the student is enrolled for the exam
            if (student == null)
            {
                Console.WriteLine($"{args[0]} has bonus points but is not enrolled for the exam");
            }
            else
            {

                decimal percentage = decimal.Parse(args[1]);
                if (percentage < 0 || percentage > 1)
                {
                    throw new ArgumentException("Invalid percentage: " + percentage);
                }
                //Get the maximum level which is not greater than the given percentage
                decimal maxKey = BonusPointsLevels.Where(x => x.Key <= percentage).Max(x => x.Key);
                //Assign the corresponding value
                student.BonusPoints = BonusPointsLevels[maxKey];
            }
        }
    }
}
