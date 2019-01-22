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
            var text = File.ReadAllLines(settings.CourseListPath);

            for (int i = 0; i < text.Length; i++)
            {
                var args = text[i].Split('\t');
                if (args.Length != 8)
                {
                    logger.LogWarning("Invalid student course list line: " + text[i]);
                }
                else
                {
                    bool success = int.TryParse(args[1], out int id);
                    if (!success)
                    {
                        logger.LogWarning($"Invalid student id '{args[1]}' in the line {text[i]}");
                    }
                    else
                    {
                        var student = students.SingleOrDefault(x => x.StudentId == id);
                        if(student != null)
                        {
                            student.FirstName = args[3];
                            student.LastName = args[2];
                            student.DegreeCourse = args[4];
                            student.Email = args[6].ToLowerInvariant();
                        }
                    }
                }
            }

            var notMatched = students.Count(x => string.IsNullOrEmpty(x.FirstName) ||
                string.IsNullOrEmpty(x.LastName) ||
                string.IsNullOrEmpty(x.DegreeCourse) ||
                string.IsNullOrEmpty(x.Email));
            logger.LogInformation($"Found {text.Length} students registered for the course. Did not find details for {notMatched} students.");
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