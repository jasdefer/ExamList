using ExamListCore.Interfaces;
using ExamListCore.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExamListCore.Implementations
{
    public class DefaultRoomListPrinter : IRoomListPrinter
    {
        private readonly Settings settings;
        private readonly ILogger<StineStudentReader> logger;

        public DefaultRoomListPrinter(IOptions<Settings> options,
            ILogger<StineStudentReader> logger)
        {
            if (options?.Value == null) throw new ArgumentNullException(nameof(options));
            settings = options.Value;
            this.logger = logger;
        }

        public void Print(IEnumerable<Student> students)
        {
            var dir = Path.GetDirectoryName(settings.SeatTablePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                logger.LogInformation("Seat table output directory not found. It was created.");
            }
            var sb = new StringBuilder();
            foreach (var student in students.OrderBy(x => x.StudentId))
            {
                sb.AppendLine($"{student.StudentId}\t{student.Room.Name}\t{student.Seat}");
            }
            File.WriteAllText(settings.SeatTablePath, sb.ToString());
        }
    }
}