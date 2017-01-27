using ExamListCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListCore.CsvReader.StudentReaders
{
    public class StineList : IStudentReader
    {
        public bool ThrowExceptionOnInvalidStudent { get; set; }

        public Student Read(string[] arguments)
        {
            if (arguments.Length != 6)
            {
                return Error("Invalid amount of arguments in a line");
            }

            //Read and validate the student number
            int studentNumber = 0;
            bool success = int.TryParse(arguments[1], out studentNumber);
            if (!success || studentNumber <= 0)
            {
                return Error("Invalid student number: " + arguments[1]);
            }

            //Assign all strings to the student
            Student student = new Student()
            {
                LastName = arguments[2],
                FirstName = arguments[3],
                StudentNumber = studentNumber
            };

            //Validate the string values of a student
            if (!student.IsValid())
            {
                return Error("Invalid data for student");
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
