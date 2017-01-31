using ExamListCore;
using ExamListCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExamListCoreTest
{
    public class DistributionTest
    {
        private readonly Room _ROOM_A = new Room() { Name = "Phil A", Capacity = 100 };
        private readonly Room _ROOM_B = new Room() { Name = "Phil B", Capacity = 100 };
        [Fact]
        public void TestDistribution()
        {
            //Validate if each seat is only taken once and if the capacity is exceeded
            Assert.True(Validator.IsValidRoomAllocation(GetDistributedStudents()));
        }

        [Fact]
        public void InvalidDistributionNoRoom()
        {
            List<Student> students = GetDistributedStudents();
            students.Last().SetSeat(null, 1);
            //Validate if each seat is only taken once and if the capacity is exceeded
            Assert.False(Validator.IsValidRoomAllocation(students));
        }

        [Fact]
        public void InvalidDistributionCapacityExceeded()
        {
            List<Student> students = GetDistributedStudents();
            Student student = new Student()
            {
                FirstName = "Jon",
                LastName = "Doe",
                StudentId = 123
            };
            student.SetSeat(_ROOM_A, 101);
            students.Add(student);
            //Validate if each seat is only taken once and if the capacity is exceeded
            Assert.False(Validator.IsValidRoomAllocation(students));
        }

        [Fact]
        public void InvalidDistributionDoubleSeat()
        {
            List<Student> students = GetDistributedStudents();

            //Increase the number of a student of the full room. This guarantees double distributed seat
            Student student =students.Where(x => x.Room == _ROOM_A && x.Seat != 1 && x.Seat != _ROOM_A.Capacity).First();
            student.SetSeat(_ROOM_A, student.Seat + 1);
            students.Add(student);
            //Validate if each seat is only taken once and if the capacity is exceeded
            Assert.False(Validator.IsValidRoomAllocation(students));
        }

        private List<Student> GetDistributedStudents()
        {
            Distributor dis = new Distributor();
            //Add rooms
            dis.AddRoom(_ROOM_A);
            dis.AddRoom(_ROOM_B);

            //Generate Students
            List<Student> students = new List<Student>();
            for (int i = 0; i < 180; i++)
            {
                students.Add(new Student()
                {
                    FirstName = "FirstName" + i,
                    LastName = "LastName" + i,
                    StudentId = i + 1
                });
            }
            dis.Students = students;
            //Distribute the students to the rooms
            dis.Distribute();

            return students;
        }
    }
}
