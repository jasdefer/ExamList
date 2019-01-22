using System.Collections.Generic;
using ExamListCore.Model;

namespace ExamListCore.Interfaces
{
    public interface IStudentDistribution
    {
        void Distribute(IEnumerable<Student> students, IEnumerable<Room> rooms);
    }
}