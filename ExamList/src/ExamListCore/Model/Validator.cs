using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListCore.Model
{
    public static class Validator
    {
        public static bool IsValidRoomAllocation(List<Student> students)
        {
            //Get all rooms from the students
            IEnumerable<Room> rooms = students.Select(x => x.Room).Distinct();

            //Check if the capacity of a room is not exceeded and no seat is distributed more than once
            Dictionary<Room, int> capacities = new Dictionary<Room, int>();
            Dictionary<Room, List<int>> seats = new Dictionary<Room, List<int>>();
            foreach (var room in rooms)
            {
                if (room == null)
                {
                    return false;
                }
                capacities.Add(room, 0);
                seats.Add(room, new List<int>());
            }

            //Check if each student is valid and update the room information
            foreach (Student student in students)
            {
                if (!student.IsValid())
                {
                    return false;
                }
                capacities[student.Room]++;
                seats[student.Room].Add(student.Seat);
            }

            //Check if the capacity of each room is exceeded
            foreach (var item in capacities)
            {
                if (item.Key.Capacity < item.Value)
                {
                    return false;
                }
            }

            //Check if no seat is distributed more than once
            foreach (var item in seats)
            {
                if (item.Value.Count != item.Value.Distinct().Count())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
