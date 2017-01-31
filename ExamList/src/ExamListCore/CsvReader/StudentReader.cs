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
    /// Reads the students registered for an exam from a csv file.
    /// </summary>
    public static class StudentReader
    {
        /// <summary>
        /// Read a csv file and convert each line to a student.
        /// </summary>
        /// <param name="path">The path of the csv file which contains the student information.</param>
        /// <param name="converter">The methods converts the arguments of a line to a student.</param>
        /// <param name="separator">The character which separates the arugments in a line.</param>
        /// <returns>The List of all converted students given in the csv file.</returns>
        private static List<Student> ReadStudents(string path, Func<string[], Student> converter, char separator = '\t')
        {
            List<Student> students = new List<Student>();
            foreach (string line in File.ReadLines(path))
            {
                string[] args = line.Split(separator);
                students.Add(converter.Invoke(args));
            }
            return students;
        }

        /// <summary>
        /// Get all students from a csv file in the format of the default STiNE exam list.
        /// </summary>
        /// <param name="path">The path of the csv file.</param>
        /// <param name="separator">The character, which separates the arguments in a line.</param>
        /// <returns>The List of all converted students given in the csv file.</returns>
        public static List<Student> FromDefaultExamList(string path, char separator = '\t')
        {
            return ReadStudents(path, ConvertFromDefaultExamList, separator);
        }

        /// <summary>
        /// Get all students from a csv file in the format of the extended STiNE exam list.
        /// </summary>
        /// <param name="path">The path of the csv file.</param>
        /// <param name="separator">The character, which separates the arguments in a line.</param>
        /// <returns>The List of all converted students given in the csv file.</returns>
        public static List<Student> FromExtendedExamList(string path, char separator = '\t')
        {
            return ReadStudents(path, ConvertFromExtendedExamList, separator);
        }

        /// <summary>
        /// Converts a line of the default STiNE exam list to a student.
        /// </summary>
        /// <param name="args">The arguments given in a single line of a csv file.</param>
        public static Student ConvertFromDefaultExamList(string[] args)
        {
            //Validate the input
            if (args.Length != 3)
            {
                throw new ArgumentException("Unexpected number of arguments in line");
            }

            //Assign the input
            Student student = new Student();
            student.StudentId = int.Parse(args[1]);

            //Validaten
            if (student.StudentId < 1)
            {
                throw new ArgumentOutOfRangeException("Student Number");
            }
            return student;
        }

        /// <summary>
        /// Converts a line of the extended STiNE exam list to a student.
        /// </summary>
        /// <param name="args">The arguments given in a single line of a csv file.</param>
        public static Student ConvertFromExtendedExamList(string[] args)
        {
            //Validate the input
            if (args.Length != 6)
            {
                throw new ArgumentException("Unexpected number of arguments in line");
            }

            //Assign the input
            Student student = new Student();
            student.StudentId = int.Parse(args[1]);
            student.DegreeCourse = args[3];

            //Validaten
            if (string.IsNullOrEmpty(student.DegreeCourse))
            {
                throw new ArgumentNullException("Degree course");
            }
            if (student.StudentId < 1)
            {
                throw new ArgumentOutOfRangeException("Student Number");
            }
            return student;
        }

        
    }
}
