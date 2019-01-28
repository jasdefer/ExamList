using ExamListCore.Interfaces;
using ExamListCore.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExamListCore.Implementations
{
    public class RandomStudentDistribution : IStudentDistribution
    {
        private readonly Settings settings;
        private readonly ILogger<StineStudentReader> logger;

        public RandomStudentDistribution(IOptions<Settings> options,
            ILogger<StineStudentReader> logger)
        {
            if (options?.Value == null) throw new ArgumentNullException(nameof(options));
            settings = options.Value;
            this.logger = logger;
        }

        public IEnumerable<Student> Distribute(IEnumerable<Student> students, IEnumerable<Room> rooms)
        {
            var rnd = new Random(settings.RandomSeed);
            var roomSeats = rooms.ToDictionary(x => x, x => 0);
            SeatAssignedStudents(students, roomSeats);
            var sortedStudents = students.OrderBy(x => rnd.Next()).ToArray();
            int index = 0;
            
            foreach (var room in rooms)
            {
                while (roomSeats[room] < room.Capacity && index<sortedStudents.Length)
                {
                    if (sortedStudents[index].Room == null)
                    {
                        sortedStudents[index].SetSeat(room, ++roomSeats[room]);
                    }
                    index++;
                }
            }
            if (rooms.Sum(x => x.Capacity) < sortedStudents.Length)
            {
                logger.LogError($"Capacity of the rooms ({rooms.Sum(x => x.Capacity)}) is not enough for {sortedStudents.Length} students.");
            }

            foreach (var roomSeat in roomSeats)
            {
                logger.LogInformation($"Seated {roomSeat.Value} students in total in the room {roomSeat.Key.Name}");
            }
            return sortedStudents.OrderBy(x => x.Seat).OrderBy(x => x.Room?.Name).ToArray();
        }

        private void SeatAssignedStudents(IEnumerable<Student> students, Dictionary<Room,int> rooms)
        {
            if (!File.Exists(settings.AssignedStudentRooms))
            {
                logger.LogInformation("No students are assigned to certain rooms.");
            }
            else
            {
                var lines = File.ReadAllLines(settings.AssignedStudentRooms);
                for (int i = 0; i < lines.Length; i++)
                {
                    var args = lines[i].Split('\t');
                    if (args.Length != 2)
                    {
                        logger.LogWarning($"Invalid AssignedStudentRooms on line {i}: {lines[i]}");
                    }
                    else
                    {
                        var success = int.TryParse(args[0], out int studentId);
                        var student = students.SingleOrDefault(x => x.StudentId == studentId);
                        if (!success || student == null)
                        {
                            logger.LogWarning($"Invalid student id {args[0]}");
                        }
                        else
                        {
                            var room = rooms.Keys.SingleOrDefault(x => string.Compare(x.Name, args[1], true)==0);
                            if (room == null)
                            {
                                logger.LogWarning($"Room '{args[1]}' not found.");
                            }
                            else
                            {
                                student.SetSeat(room, ++rooms[room]);
                            }
                        }
                    }
                }
            }

            foreach (var room in rooms.Where(x => x.Value>0))
            {
                logger.LogInformation($"Seated {room.Value} students directly in the room {room.Key.Name}");
            }
        }
    }
}