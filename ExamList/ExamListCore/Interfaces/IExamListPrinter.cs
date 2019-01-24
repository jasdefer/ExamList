using System.Collections.Generic;
using ExamListCore.Model;

namespace ExamListCore.Interfaces
{
    public interface IExamListPrinter
    {
        void Print(IEnumerable<Student> students);
    }
}