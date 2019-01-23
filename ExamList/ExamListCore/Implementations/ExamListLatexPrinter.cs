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
    public class ExamListLatexPrinter : IExamListPrinter
    {
        private readonly Settings settings;
        private readonly ILogger<StineStudentReader> logger;

        public ExamListLatexPrinter(IOptions<Settings> options,
            ILogger<StineStudentReader> logger)
        {
            if (options?.Value == null) throw new ArgumentNullException(nameof(options));
            settings = options.Value;
            this.logger = logger;
        }

        public void Print(IEnumerable<Student> students)
        {
            if (settings.GroupCount < 1)
            {
                logger.LogWarning("Invalid group count. Assuming only 1 group.");
                settings.GroupCount = 1;
            }

            var dir = Path.GetDirectoryName(settings.LatexPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                logger.LogInformation("Latex output directory not found. It was created.");
            }
            var sb = new StringBuilder();
            int group = 1;
            foreach (var student in students)
            {
                sb.AppendLine($@"\matrikelnummer{{{student.StudentId}}}{{{student.Room.Name}}}{{{student.Seat}}}{{{student.LastName}}}{{{student.FirstName}}}{{{student.BonusPoints}}}{{{group}}}{{{student.DegreeCourse}}}");
                group++;
                if (group > settings.GroupCount) group = 1;
            }
            File.WriteAllText(settings.LatexPath, sb.ToString());
        }
    }
}