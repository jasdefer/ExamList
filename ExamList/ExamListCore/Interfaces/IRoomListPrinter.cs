using ExamListCore.Model;
using System.Collections.Generic;

namespace ExamListCore.Interfaces
{
    public interface IRoomListPrinter
    {
        void Print(IEnumerable<Student> students);
    }
}