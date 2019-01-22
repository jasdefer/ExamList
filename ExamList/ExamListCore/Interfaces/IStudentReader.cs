using ExamListCore.Model;
using System.Collections.Generic;

namespace ExamListCore.Interfaces
{
    public interface IStudentReader
    {
        IEnumerable<Student> ReadStudents();
    }
}