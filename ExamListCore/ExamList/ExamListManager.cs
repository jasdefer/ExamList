using ExamList.Interfaces;
using ExamList.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamList
{
    public class ExamListManager
    {
        private readonly IStudentReader _StudentReader;
        private readonly IRoomReader _RoomReader;
        private readonly IBonusPointReader _BonusPointReader;
        private readonly IExamListPrinter _ExamListPrinter;

        public ExamListManager(IStudentReader studentReader,
            IRoomReader roomReader,
            IBonusPointReader bonusPointReader,
            IExamListPrinter examListPrinter)
        {
            _StudentReader = studentReader ?? throw new ArgumentNullException(nameof(studentReader));
            _RoomReader = roomReader ?? throw new ArgumentNullException(nameof(roomReader));
            _BonusPointReader = bonusPointReader ?? throw new ArgumentNullException(nameof(bonusPointReader));
            _ExamListPrinter = examListPrinter ?? throw new ArgumentNullException(nameof(examListPrinter));
        }

        public void Start(bool hasBonus, Random rnd = null)
        {
            //Read data
            rnd = rnd ?? new Random(1);
            IEnumerable<Student> students = _StudentReader.Read();
            IEnumerable<Room> rooms = _RoomReader.Read();
            if (students.Count() > rooms.Sum(x => x.Capacity)) throw new Exception("Students exceed capacity.");

            //Assign students a random room and seat
            students = students.OrderBy(x => rnd.Next());
            int index = 0;
            int counter = 0;
            foreach (Student student in students)
            {
                if (hasBonus)
                {
                    student.BonusPoints = _BonusPointReader.Read(student);
                }
                Room room = rooms.ElementAt(index);
                student.SetSeat(room, counter+1);
                counter++;
                if(counter> room.Capacity)
                {
                    index++;
                    counter = 0;
                }
            }
            _ExamListPrinter.Print(students);
        }
    }
}