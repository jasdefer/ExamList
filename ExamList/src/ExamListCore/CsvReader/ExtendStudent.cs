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
            //Check which students have been extended
            List<int> studentIds = students.Select(x => x.StudentId).ToList();

            //Read all lines from the csv file
            foreach (var line in File.ReadLines(path))
            {
                string[] args = line.Split(separator);
                //Find the corresponding student, the id is always given in the second row
                int studentId = int.Parse(args[1]);
                Student student = students.SingleOrDefault(x => x.StudentId == studentId);

                //Check if the student from the course is registered to the exam
                if (student != null)
                {
                    //Add the detailed information to the student
                    Extender(student, args);
                    studentIds.Remove(studentId);
                }
                else
                {
                    Console.WriteLine("Bonus Points, but not enrolled for the exam: " + studentId);
                }
            }

            //Some students may be enrolled for an exam without beeing registered for the course
            if (studentIds.Count > 0)
            {
                Console.WriteLine($"Did not find information about {studentIds.Count} student in the course file. Following student ids:");
                foreach (var item in studentIds)
                {
                    Console.WriteLine("" + item);
                }
            }
        }

        public static void ExtendFromDefaultList(object _ExtendStudentsPath, List<Student> students, char _Separator)
        {
            throw new NotImplementedException();
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
                throw new ArgumentException("Unexpected number of arguments in line of the default course list. Expected 5 found "+args.Length);
            }

            if (student.StudentId != int.Parse(args[1]))
            {
                throw new Exception($"Given line does not match the student {string.Join(";",args)}");
            }

            //Assign the input
            student.LastName = args[2];
            student.FirstName = args[3];

            if(string.IsNullOrEmpty(student.FirstName) || string.IsNullOrEmpty(student.LastName))
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
            if (student == null)
            {
                throw new ArgumentNullException("student");
            }
            if(args.Length != 11)
            {
                throw new ArgumentException("Unexpected number of arguments in line of the extended course list. Expected 11 found " + args.Length);
            }
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
                //Check if the no degree course was given at all
                if (string.IsNullOrEmpty(student.DegreeCourse))
                {
                    throw new ArgumentException($"Degree Course not defined for student id {student.StudentId}");
                }
            }
            else if (student.DegreeCourse != args[4])
            {
                throw new ArgumentException($"The stored degree course ({student.DegreeCourse}) does not match the degree course from the csv file ({args[4]}) ");
            }

            //Assign the name
            student.LastName = args[2];
            student.FirstName = args[3];
            if(string.IsNullOrEmpty(student.LastName) || string.IsNullOrEmpty(student.FirstName))
            {
                throw new ArgumentException("No first name or last name found");
            }
            //Assign the email address and validate it
            student.Email = args[6];
            if (string.IsNullOrEmpty(student.Email) || !student.Email.Contains("@"))
            {
                throw new Exception("Invalid Email: "+args[6]);
            }

        }
    }
}
