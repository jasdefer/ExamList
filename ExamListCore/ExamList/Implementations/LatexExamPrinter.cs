using ExamList.Interfaces;
using ExamList.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExamList.Implementations
{
    /// <summary>
    /// Print the student list to file used by the Latex exam template
    /// \matrikelnummer{123456}{Audimax}{12}{Mustermann}{Max}{4.5}{HWI}
    /// </summary>
    public class LatexExamPrinter : IExamListPrinter
    {
        private readonly string _OutputPath;
        private readonly ILogger<LatexExamPrinter> _Logger;

        public LatexExamPrinter(string outputPath, ILogger<LatexExamPrinter> logger)
        {
            _OutputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Print(IEnumerable<Student> students)
        {
            if (students == null) throw new ArgumentNullException(nameof(students));

            //Ensure directory created
            string directory = Path.GetDirectoryName(_OutputPath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            //Build file content
            StringBuilder sb = new StringBuilder();
            foreach (Room room in students.Select(s => s.Room)
                .OrderBy(r => r.Name)
                .Distinct())
            {
                sb.AppendLine("%"+room.Name);
                foreach (Student student in students
                    .OrderBy(s => s.Seat)
                    .Where(s => s.Room == room))
                {
                    sb.AppendLine($@"\matrikelnummer{{{student.StudentId}}}{{{student.Room.Name}}}{{{student.Seat}}}{{{student.LastName}}}{{{student.FirstName}}}{{{student.BonusPoints}}}{{{student.DegreeCourse}}}");
                }
            }
            
            //Write file
            File.WriteAllText(_OutputPath, sb.ToString());
            _Logger.LogInformation("Completed printing the file to " + _OutputPath);
        }
    }
}