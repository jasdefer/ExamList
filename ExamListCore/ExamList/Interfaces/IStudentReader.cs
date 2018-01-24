using System.Collections.Generic;
using ExamList.Model;

namespace ExamList.Interfaces
{
    public interface IStudentReader
    {
        IEnumerable<Student> Read();
    }
}