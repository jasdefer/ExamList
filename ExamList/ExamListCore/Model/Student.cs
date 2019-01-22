namespace ExamListCore.Model
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DegreeCourse { get; set; }
        public decimal BonusPoints { get; set; }
        public int Seat { get; private set; }
        public Room Room { get; private set; }

        public Student(int id)
        {
            StudentId = id;
        }

        public void SetSeat(Room room, int seat)
        {
            Room = room;
            Seat = seat;
        }
    }
}