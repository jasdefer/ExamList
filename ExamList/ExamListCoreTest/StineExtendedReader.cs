using ExamListCore.CsvReader;
using ExamListCore.CsvReader.StudentReaders;
using ExamListCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExamListCoreTest
{
    public class StineExtendedReader
    {
        [Fact]
        public void ReadLineCorrectly()
        {
            List<string[]> students = GetValidStrings();
            StineExtendedList reader = new StineExtendedList()
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
            StineExtendedList reader = new StineExtendedList()
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
            StineExtendedList reader = new StineExtendedList()
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
                new string[] { "1","6655510","Jon","Smith","BWL (B.Sc.) - WiSe 10/10","Modell-Bezug","jon.smith@studium.uni-hamburg.de","Unternehmensführung","Bachelor of Science","5",""},
                new string[] { "2","6795481", "Jimena Karla", "Cummings", "Wirtschaftsingenieurwesen (B.Sc.)", "Modell-Bezug", "jimena.karla.cummings@studium.uni-hamburg.de","","Bachelor of Science","3",""},
                new string[] {"3","1233892", "Phoenix", "Eaton", "Wirtschaftsmathematik (B.Sc.) - WiSe 10/10", "Modell-Bezug", "phoenix.eaton@studium.uni-hamburg.de","","Bachelor of Science","7","" },
            };
        }

        private List<string[]> GetInvalidStrings()
        {
            return new List<string[]>()
            {
                new string[] { "1","-1","Jon","Smith","BWL (B.Sc.) - WiSe 10/10","Modell-Bezug","jon.smith@studium.uni-hamburg.de","Unternehmensführung","Bachelor of Science","5",""},
                new string[] { "2","", "Jimena Karla", "Cummings", "Wirtschaftsingenieurwesen (B.Sc.)", "Modell-Bezug", "jimena.karla.cummings@studium.uni-hamburg.de","","Bachelor of Science","3",""},
                new string[] {"3","1233892", "", "Eaton", "Wirtschaftsmathematik (B.Sc.) - WiSe 10/10", "Modell-Bezug", "phoenix.eaton@studium.uni-hamburg.de","","Bachelor of Science","7" ,""},
                new string[] { "1","6655510","Jon","","BWL (B.Sc.) - WiSe 10/10","Modell-Bezug","jon.smith@studium.uni-hamburg.de","Unternehmensführung","Bachelor of Science","5", "" }
            };
        }
    }
}
