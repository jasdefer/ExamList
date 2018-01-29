using ExamList.Interfaces;
using ExamList.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExamList
{
    /// <summary>
    /// Read students enrolled for a exam. Optionally, add bonus points. Shuffle the students and distribute them to the available rooms.
    /// </summary>
    public class ExamListManager
    {
        private readonly IStudentReader _StudentReader;
        private readonly IRoomReader _RoomReader;
        private readonly IBonusPointReader _BonusPointReader;
        private readonly IExamListPrinter _ExamListPrinter;
        private readonly ILogger<ExamListManager> _Logger;

        /// <param name="studentReader">Reads the students enrolled for an exam.</param>
        /// <param name="roomReader">Reads the available rooms including their capacity.</param>
        /// <param name="examListPrinter">Prints the student list in the required format.</param>
        /// <param name="bonusPointReader">Reads bonus points achieved by the students.</param>
        public ExamListManager(IStudentReader studentReader,
            IRoomReader roomReader,
            IExamListPrinter examListPrinter,
            ILogger<ExamListManager> logger,
            IBonusPointReader bonusPointReader = null)
        {
            _StudentReader = studentReader ?? throw new ArgumentNullException(nameof(studentReader));
            _RoomReader = roomReader ?? throw new ArgumentNullException(nameof(roomReader));
            _ExamListPrinter = examListPrinter ?? throw new ArgumentNullException(nameof(examListPrinter));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _BonusPointReader = bonusPointReader;
        }

        public void Start(bool hasBonus, Random rnd = null)
        {
            rnd = rnd ?? new Random(1);
            if (hasBonus && _BonusPointReader == null) throw new Exception("Please provide a bonus points reader.");

            //Read the students and rooms
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
                student.SetSeat(room, counter+1+room.Offset);
                counter++;
                if(counter >= room.Capacity-1)
                {
                    index++;
                    counter = 0;
                }
            }

            if (hasBonus) _Logger.LogInformation($"Found records of bonus points for {_BonusPointReader.StudentWithBonuspointRecord}/{students.Count()} students.");

            //Print the results
            _ExamListPrinter.Print(students);
        }
    }
}