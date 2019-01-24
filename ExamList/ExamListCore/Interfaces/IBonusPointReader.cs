using System.Collections.Generic;
using ExamListCore.Model;

namespace ExamListCore.Interfaces
{
    public interface IBonusPointReader
    {
        void ReadBonusPoints(IEnumerable<Student> students);
    }
}