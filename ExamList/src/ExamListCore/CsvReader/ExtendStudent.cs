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
    /// Reads detailed information about a student from a csv file.
    /// </summary>
    public static class ExtendStudent
    {
        /// <summary>
        /// Read the detailed information about the students and add them to the list of students who are registered for an exam. Students not found in the given file are most likely registered for an exam but not for the course. Their detailed information cannot be retrieved and is left empty.
        /// </summary>
        /// <param name="path">The path to the csv file with detailed information</param>
        /// <param name="students">The list of students registered for an exam.</param>
        /// <param name="Extender">The function retrieving information from the given file</param>
        /// <param name="separator">The character which separates the arguments in a line of the csv file.</param>
        private static void AddInformation(string path, List<Student> students, Action<Student,string[]> Extender, char separator = '\t')
        {
            //Read all lines from the csv file
            foreach (var line in File.ReadLines(path))
            {
                string[] args = line.Split(separator);
                //Find the corresponding student, the id is always given in the second row
                int studentId = int.Parse(args[1]);
                Student student = students.SingleOrDefault(x => x.StudentId == studentId);

                //Check if the student is found
                if (student == null)
                {
                    //Student was registered in the exam but not in the course. Leave his detailed information blank and notify the user
                    Console.WriteLine($"Student {student.StudentId} not found in {Path.GetFileName(path)}.");
                }
                else
                {
                    //Add the detailed information to the student
                    Extender(student, args);
                }
            }
        }

        /// <summary>
        /// Read the detailed information about the students from the default course list in STiNE and add them to the list of students who are registered for an exam. Students not found in the given file are most likely registered for an exam but not for the course. Their detailed information cannot be retrieved and is left empty.
        /// </summary>
        /// <param name="path">The path to the csv file with detailed information</param>
        /// <param name="students">The list of students registered for an exam.</param>
        /// <param name="Extender">The function retrieving information from the given file</param>
        /// <param name="separator">The character which separates the arguments in a line of the csv file.</param>
        public static void ExtendFromDefaultList(string path, List<Student> students, char separator = '\t')
        {
            AddInformation(path, students, ExtendFromDefaultList, separator);
        }


        /// <summary>
        /// Read the detailed information about the students from the extended course list in STiNE and add them to the list of students who are registered for an exam. Students not found in the given file are most likely registered for an exam but not for the course. Their detailed information cannot be retrieved and is left empty.
        /// </summary>
        /// <param name="path">The path to the csv file with detailed information</param>
        /// <param name="students">The list of students registered for an exam.</param>
        /// <param name="Extender">The function retrieving information from the given file</param>
        /// <param name="separator">The character which separates the arguments in a line of the csv file.</param>
        public static void ExtendFromExtendedList(string path, List<Student> students, char separator = '\t')
        {
            AddInformation(path, students, ExtendFromExtendedList, separator);
        }

        /// <summary>
        /// Add information from the arguments of the default STiNE exam list to a student
        /// </summary>
        /// <param name="student">The student </param>
        /// <param name="args">The arguments of the matching line for the given student from the default STiNE course csv file.</param>
        public static void ExtendFromDefaultList(Student student, string[] args)
        {

            //Validate the input
            if (args.Length != 5)
            {
                throw new ArgumentException("Unexpected number of arguments in line");
            }

            if (student.StudentId != int.Parse(args[1]))
            {
                throw new Exception($"Given line does not match the student {string.Join(";",args)}");
            }

            //Assign the input
            student.LastName = args[2];
            student.FirstName = args[3];

            if(string.IsNullOrEmpty(student.LastName) || string.IsNullOrEmpty(student.LastName))
            {
                throw new ArgumentException("No first name or last name found");
            }
        }

        /// <summary>
        /// Add information from the arguments of the extended STiNE exam list to a student
        /// </summary>
        /// <param name="student">The student </param>
        /// <param name="args">The arguments of the matching line for the given student from the extended STiNE course csv file.</param>
        public static void ExtendFromExtendedList(Student student, string[] args)
        {
            //Validate the input
            if (student.StudentId != int.Parse(args[1]))
            {
                throw new Exception($"Given line does not match the student {string.Join(";", args)}");
            }

            //Assign the input

            //Add the degree course, if it is already given compare it
            if (string.IsNullOrEmpty(student.DegreeCourse))
            {
                student.DegreeCourse = args[4];
            }
            else if (student.DegreeCourse != args[4])
            {
                throw new ArgumentException($"The stored degree course ({student.DegreeCourse}) does not match the degree course from the csv file ({args[4]}) ");
            }

            //Assign the name
            student.LastName = args[2];
            student.FirstName = args[3];

            //Assign the email address and validate it
            student.Email = args[6];
            if (string.IsNullOrEmpty(student.Email) || !student.Email.Contains("@"))
            {
                throw new Exception("Invalid Email: "+args[6]);
            }

        }
    }
}
