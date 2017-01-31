using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamListCore.Model;
using System.IO;

namespace ExamListCore.Latex
{
    public static class ExamConverter
    {
        public static string PrintLatexStudentList(List<Student> students)
        {
            //Get all rooms from the students
            IEnumerable<Room> rooms = students.Select(x => x.Room).Distinct();
            string content = string.Empty;
            int counter = 0;
            foreach (var room in rooms)
            {
                content += "% " + room.Name + "\n";
                foreach (var student in students.Where(x => x.Room == room))
                {
                    //Alternate between group 1=A and 2=B
                    int group = (counter % 2 == 0 ? 1 : 2);
                    content +=$@"\matrikelnummer{{{student.StudentId}}}{{{room.Name}}}{{{student.Seat}}}{{{student.LastName}}}{{{student.FirstName}}}{{{student.BonusPoints}}}{{{group}}}{{{student.DegreeCourse}}}"+"\n";
                    counter++;
                }
                content += "\n";
            }

            content = content.TrimEnd('\n');

            return content;
        }

        public static void PrintLatexStudentList(string path, List<Student> students)
        {
            //Check the directory
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException(directory);
            }

            //Write text
            File.WriteAllText(path, PrintLatexStudentList(students));
        }

    }
}
