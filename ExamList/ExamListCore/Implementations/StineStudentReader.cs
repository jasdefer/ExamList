using ExamListCore.Interfaces;
using ExamListCore.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExamListCore.Implementations
{
    /// <summary>
    /// Read the registered students for an exam from the exam list file.
    /// Find additional information (name, email, degree, etc.) in the separate course list file.
    /// </summary>
    public class StineStudentReader : IStudentReader
    {
        private readonly Settings settings;
        private readonly ILogger<StineStudentReader> logger;

        public StineStudentReader(IOptions<Settings> options,
            ILogger<StineStudentReader> logger)
        {
            if (options?.Value == null) throw new ArgumentNullException(nameof(options));
            settings = options.Value;
            this.logger = logger;
        }

        public IEnumerable<Student> ReadStudents()
        {
            if (!File.Exists(settings.ExamListPath)) throw new FileNotFoundException("Student list file not found.");
            var students = ReadExamList();

            if (File.Exists(settings.CourseListPath))
            {
                ReadCourseList(students);
            }
            else
            {
                logger.LogWarning("Course list not found. Cannot read student details.");
            }

            return students;
        }

        private void ReadCourseList(List<Student> students)
        {
            var text = File.ReadAllText(settings.CourseListPath);
        }

        private List<Student> ReadExamList()
        {
            var text = File.ReadAllLines(settings.ExamListPath);
            var students = new List<Student>();

            if (text.Length < 2)
            {
                logger.LogWarning("No students found.");
                return students;
            }
            for (int i = 1; i < text.Length; i++)
            {
                var args = text[i].Split(';');
                if (args.Length < 1)
                {
                    logger.LogWarning("Invalid exam list line: " + text[i]);
                }
                else
                {
                    bool success = int.TryParse(args[1].Replace("\"", ""), out int id);
                    if (!success)
                    {
                        logger.LogWarning($"Cannot read the student id '{args[1]}' from line {i}={text[i]}");
                    }
                    else
                    {
                        students.Add(new Student(id));
                    }
                }
            }

            logger.LogInformation($"Found {students.Count} students registered for the exam.");
            return students;
        }
    }
}