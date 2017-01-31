using ExamListCore.CsvReader;
using ExamListCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExamListCoreTest
{
    public class ExtendStudentTest
    {
        [Theory]
        [InlineData("1;1;LastName;FirstName;1")]
        [InlineData("1;1;Doe;Jon;1")]
        public void ExtendFromDefaultListTest(string line)
        {
            string[] args = line.Split(';');
            Student student = new Student() { StudentId = 1 };
            ExtendStudent.ExtendFromDefaultList(student, args);
            Assert.NotNull(student.FirstName);
            Assert.NotNull(student.LastName);
        }

        [Theory]
        [InlineData("1;2;FirstName;LastName;1")]
        [InlineData("1;;FirstName;LastName;1")]
        [InlineData("1;0;FirstName;LastName;1")]
        [InlineData("1;1;;LastName;1")]
        [InlineData("1;1;FirstName;;1")]
        [InlineData("1;1;FirstName;LastName;1;")]
        [InlineData("1;1;FirstName;LastName")]
        public void ExtendFromDefaultListTestFail(string line)
        {
            string[] args = line.Split(';');
            Student student = new Student() { StudentId = 1 };
            bool check = false;
            try
            {
                ExtendStudent.ExtendFromDefaultList(student, args);
                //No exception thrown
                check = true;
            }
            catch (Exception)
            {
                //Exception catched as expected
            }
            Assert.False(check);
        }

        [Theory]
        [InlineData("1;1;LastName;FirstName;Degree;a;mail@gmail.com;c;d;e")]
        [InlineData("1;1;Doe;Jon;Degree;a;mail@gmail.com;c;d;e")]
        public void ExtendFromExtendedTest(string line)
        {
            string[] args = line.Split(';');
            Student student = new Student() { StudentId = 1 };
            ExtendStudent.ExtendFromExtendedList(student, args);
            Assert.NotNull(student.FirstName);
            Assert.NotNull(student.LastName);
            Assert.NotNull(student.DegreeCourse);
        }

        [Theory]
        [InlineData("1;1;Doe;Jon;Degree;a;mail@gmail.com;c;d")]
        [InlineData("1;1;Doe;Jon;Degree;a;mail@gmail.com;c;d;e;")]
        [InlineData("1;2;Doe;Jon;Degree;a;mail@gmail.com;c;d;e")]
        [InlineData("1;0;Doe;Jon;Degree;a;mail@gmail.com;c;d;e")]
        [InlineData("1;;Doe;Jon;Degree;a;mail@gmail.com;c;d;e")]
        [InlineData("1;-1;Doe;Jon;Degree;a;mail@gmail.com;c;d;e")]
        [InlineData("1;1;Doe;Jon;;a;mail@gmail.com;c;d;e")]
        [InlineData("1;1;;Jon;Degree;a;mail@gmail.com;c;d;e")]
        [InlineData("1;1;Doe;;Degree;a;mail@gmail.com;c;d;e")]
        [InlineData("1;1;Doe;Jon;Degree;a;mailgmail.com;c;d;e")]
        [InlineData("1;1;Doe;Jon;Degree;a;;c;d;e")]
        public void ExtendFromExtendedListTestFail(string line)
        {
            string[] args = line.Split(';');
            Student student = new Student() { StudentId = 1 };
            bool check = false;
            try
            {
                ExtendStudent.ExtendFromExtendedList(student, args);
                //No exception thrown
                check = true;
            }
            catch (Exception)
            {
                //Exception catched as expected
            }
            Assert.False(check);
        }
    }
}
