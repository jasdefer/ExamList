using System.Collections.Generic;
using ExamListCore.Model;

namespace ExamListCore.Interfaces
{
    public interface IStudentDistribution
    {
        IEnumerable<Student> Distribute(IEnumerable<Student> students, IEnumerable<Room> rooms);
    }
}