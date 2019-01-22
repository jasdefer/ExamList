using ExamListCore.Interfaces;
using ExamListCore.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
            var sortedStudents = students.OrderBy(x => rnd.Next()).ToArray();
            int index = 0;
            int seat = 0;
            foreach (var room in rooms)
            {
                while (seat <= room.Capacity)
                {
                    sortedStudents[index].SetSeat(room, seat);
                    seat++;
                }
            }

            return sortedStudents.OrderBy(x => x.Seat).OrderBy(x => x.Room);
        }
    }
}