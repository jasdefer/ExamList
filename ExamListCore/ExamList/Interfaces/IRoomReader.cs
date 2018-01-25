using System.Collections.Generic;
using ExamList.Model;

namespace ExamList.Interfaces
{
    public interface IRoomReader
    {
        IEnumerable<Room> Read();
    }
}