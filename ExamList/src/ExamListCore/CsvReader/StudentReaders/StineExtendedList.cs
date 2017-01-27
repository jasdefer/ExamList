using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamListCore.Model;

namespace ExamListCore.CsvReader.StudentReaders
{
    public class StineExtendedList : IStudentReader
    {
        public bool ThrowExceptionOnInvalidStudent { get; set; } = true;

        public Student Read(string[] arguments)
        {
            if (arguments.Length > 11)
            {
                return Error($"Invalid amount of arguments ({arguments.Length}) in a line.");
            }

            //Read and validate the student number
            int studentNumber = 0;
            bool success = int.TryParse(arguments[1], out studentNumber);
            if (!success || studentNumber<=0)
            {
                return Error("Invalid student number: " + arguments[1]);
            }

            //Assign all strings to the student
            Student student = new Student()
            {
                LastName = arguments[2],
                FirstName = arguments[3],
                Email = arguments[6],
                DegreeCourse = arguments[8],
                StudentNumber = studentNumber
            };


            //Validate the string values of a student
            if (!student.IsValid())
            {
                return Error("Invalid data for student");
            }

            if (string.IsNullOrEmpty(student.DegreeCourse))
            {
                return Error("Invalid degree course: "+student.DegreeCourse);
            }

            if(string.IsNullOrEmpty(student.Email) || !student.Email.Contains("@"))
            {
                return Error("Invalid email address: " + student.Email);
            }

            return student;
        }

        private Student Error(string message)
        {
            Console.WriteLine("Error reading Student list: " + message);
            if (ThrowExceptionOnInvalidStudent)
            {
                throw new ArgumentException(message);
            }
            else
            {
                return null;
            }
        }
    }
}
