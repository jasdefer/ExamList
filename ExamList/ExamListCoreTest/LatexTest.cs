using ExamListCore;
using ExamListCore.Latex;
using ExamListCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExamListCoreTest
{
    public class LatexTest
    {
        [Fact]
        public void CheckSyntax()
        {
            List<Student> students = new List<Student>();
            for (int i = 0; i < 180; i++)
            {
                Student student = new Student()
                {
                    FirstName = "FirstName" + i,
                    LastName = "LastName" + i,
                    StudentId = i + 1,
                };
                students.Add(student);
            }

            Distributor distributor = new Distributor();
            distributor.Students = students;
            distributor.AddRoom("Phil A", 100);
            distributor.AddRoom("Phil B", 100);
            distributor.Distribute(1);

            string content = ExamConverter.PrintLatexStudentList(students);
            string[] lines = content.Split('\n');
            Assert.Equal(180+2+1, lines.Count());
            foreach (var line in lines)
            {
                Assert.True(string.IsNullOrEmpty(line) || line.StartsWith("%") || line.StartsWith(@"\matrikelnummer"));
            }
        }
    }
}
