using ExamListCore.Model;
using System.Collections.Generic;

namespace ExamListCore.Interfaces
{
    public interface IRoomReader
    {
        IEnumerable<Room> ReadRooms();
    }
}