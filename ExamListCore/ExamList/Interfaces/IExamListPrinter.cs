using System.Collections.Generic;
using ExamList.Model;

namespace ExamList.Interfaces
{
    public interface IExamListPrinter
    {
        void Print(IEnumerable<Student> students);
    }
}