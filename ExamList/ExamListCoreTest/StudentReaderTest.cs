using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExamListCore.CsvReader;
using Xunit;
using ExamListCore.Model;

namespace ExamListCoreTest
{
    public class StudentReaderTest
    {
    
        [Theory]
        [InlineData("1;1;Some Name")]
        [InlineData("1;10;Some Name")]
        [InlineData("1;2147483647;Some Name")]
        public void StudentReaderDefaultTest(string line)
        {
            string[] input = line.Split(';');
            Student student = StudentReader.ConvertFromDefaultExamList(input);
            Assert.Null(student.DegreeCourse);
            Assert.Null(student.FirstName);
            Assert.Null(student.LastName);
            Assert.Null(student.Email);
            Assert.Null(student.Room);
            Assert.Equal(0, student.BonusPoints);
            Assert.Equal(0, student.Seat);
            Assert.Equal(int.Parse(input[1]), student.StudentId);
        }

        [Theory]
        [InlineData("1;2147483647;Some Name;")]
        [InlineData("1;2147483647")]
        [InlineData("2147483647")]
        [InlineData("1;0;Some Name")]
        [InlineData("1;-10;Some Name")]
        [InlineData("1;3.5;Some Name")]
        [InlineData("1;nonumber;Some Name")]
        [InlineData("1;;Some Name")]
        public void StudentReaderDefaultTestException(string line)
        {
            string[] input = line.Split(';');
            bool check = false;
            try
            {
                Student student = StudentReader.ConvertFromDefaultExamList(input);
                //Should not happen, because an exception should be thrown
                check = true;
            }
            catch (Exception)
            {
                //Exception thrown as expected
            }
            Assert.False(check);
        }

        [Theory]
        [InlineData("1;1;Some Name;Degree;Context;100%")]
        [InlineData("1;10;Some Name;Degree;Context;100%")]
        [InlineData("1;2147483647;Some Name;Degree;Context;100%")]
        public void StudentReaderExtendedTest(string line)
        {
            string[] input = line.Split(';');
            Student student = StudentReader.ConvertFromExtendedExamList(input);
            Assert.Null(student.FirstName);
            Assert.Null(student.LastName);
            Assert.Null(student.Email);
            Assert.Null(student.Room);
            Assert.Equal(input[3],student.DegreeCourse);
            Assert.Equal(0, student.BonusPoints);
            Assert.Equal(0, student.Seat);
            Assert.Equal(int.Parse(input[1]), student.StudentId);
        }

        [Theory]
        [InlineData("1;1;Some Name;Degree;Context")]
        [InlineData("1;1;Some Name;Degree")]
        [InlineData("1;1;Some Name")]
        [InlineData("1;1")]
        [InlineData("1;1;Some Name;;Context;100%")]
        [InlineData("1;;Some Name;Degree;Context;100%")]
        [InlineData("1;-1;Some Name;Degree;Context;100%")]
        [InlineData("1;0;Some Name;Degree;Context;100%")]
        [InlineData("1;3.5;Some Name;Degree;Context;100%")]
        public void StudentReaderExtendedTestException(string line)
        {
            string[] input = line.Split(';');
            bool check = false;
            try
            {
                Student student = StudentReader.ConvertFromExtendedExamList(input);
                //Should not happen, because an exception should be thrown
                check = true;
            }
            catch (Exception)
            {
                //Exception thrown as expected
            }
            Assert.False(check);
        }
    }
}

