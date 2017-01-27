using ExamListCore.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListCore.CsvReader
{
    public static class CsvReader
    {
        public static List<Student> ReadStudents(string path, char separator, IStudentReader reader)
        {
            List<Student> students = new List<Student>();
            //Iterate over all lines in the file
            foreach (string line in File.ReadLines(path))
            {
                //Get the columns of the csv file
                string[] arguments = line.Split(separator);
                //Convert the arguments to a student. 
                Student student = reader.Read(arguments);
                //Ignore the line, if it contains invalid data
                if(student!=null)
                {
                    students.Add(student);
                }
            }

            return students;

        }
    }
}
