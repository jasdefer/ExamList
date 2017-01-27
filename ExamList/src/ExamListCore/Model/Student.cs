using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListCore.Model
{
    public class Student
    {
        public int StudentNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string DegreeCourse { get; set; }
        public int Seat { get; private set; }
        public Room Room { get; private set; }

        /// <summary>
        /// Validate the fundamental data of a student.
        /// </summary>
        /// <returns>True, if the student number, the first and last name is valid.</returns>
        public bool IsValid()
        {
            if (StudentNumber <= 0)
            {
                return false;
            }
            if (string.IsNullOrEmpty(LastName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(FirstName))
            {
                return false;
            }
            //All other properties are not required

            return true;
        }

        public void SetSeat(Room room, int seat)
        {
            Room = room;
            Seat = seat;
        }

    }
}
