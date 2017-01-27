using ExamListCore.CsvReader.StudentReaders;
using ExamListCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExamListCoreTest
{
    public class StineReader
    {
        [Fact]
        public void ReadLineCorrectly()
        {
            List<string[]> students = GetValidStrings();
            StineList reader = new StineList()
            {
                ThrowExceptionOnInvalidStudent = false
            };
            foreach (string[] arguments in students)
            {
                Student student = reader.Read(arguments);
                Assert.NotNull(student);
            }
        }

        [Fact]
        public void DetectNullReturns()
        {
            List<string[]> students = GetInvalidStrings();
            StineList reader = new StineList()
            {
                ThrowExceptionOnInvalidStudent = false
            };
            foreach (string[] arguments in students)
            {
                Student student = reader.Read(arguments);
                Assert.Null(student);
            }
        }

        [Fact]
        public void DetectThrownExceptionsReturns()
        {
            List<string[]> students = GetInvalidStrings();
            StineList reader = new StineList()
            {
                ThrowExceptionOnInvalidStudent = true
            };
            foreach (string[] arguments in students)
            {
                try
                {
                    Student student = reader.Read(arguments);
                    Assert.True(false);
                }
                catch(ArgumentException)
                {

                }
                
            }
        }

        private List<string[]> GetValidStrings()
        {
            return new List<string[]>()
            {
                new string[] { "1","9858711","Phoenix","Eaton","","Übung zu Grundlagen des Operations Research - Z1 - 2", },
                new string[] { "2","5783289", "Karla  Elian ", "Cowan", "","Übung zu Grundlagen des Operations Research - Z1 - 1 (NUR FÜR HWI)"},
                new string[] {"3","1263898", "Evie", "Ward","","Übung zu Grundlagen des Operations Research - Z1 - 2" }
            };
        }

        private List<string[]> GetInvalidStrings()
        {
            return new List<string[]>()
            {
                new string[] { "1","","Phoenix","Eaton","","Übung zu Grundlagen des Operations Research - Z1 - 2", },
                new string[] { "1","-1","Phoenix","Eaton","","Übung zu Grundlagen des Operations Research - Z1 - 2", },
                new string[] { "1","0","Phoenix","Eaton","","Übung zu Grundlagen des Operations Research - Z1 - 2", },
                new string[] { "2","5783289", "", "Cowan", "","Übung zu Grundlagen des Operations Research - Z1 - 1 (NUR FÜR HWI)"},
                new string[] {"3","1263898", "Evie", "","","Übung zu Grundlagen des Operations Research - Z1 - 2" }
            };
        }
    }
}
