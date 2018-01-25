using ExamList.Interfaces;
using ExamList.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExamList.Implementations
{
    /// <summary>
    /// Reads the student enrolled for the exam and read student details from the course participants.
    /// The exam file looks like
    /// "1";"123456";"Max Mustermann";"HWI";"Operations Research";"100%"
    /// The course details file looks like:
    /// 1\t123456\tMax\tMusteramnn\tHWI\tOperations Research\tMax.Mustermann@uni-hamburg.de\tStatistik\tBachelor of Science\t3
    /// </summary>
    public class StineStudentReader : IStudentReader
    {
        private readonly string _ExamFilePath;
        private readonly string _CourseFilePath;
        private readonly ILogger<StineStudentReader> _Logger;

        public StineStudentReader(string examFilePath, string courseFilePath, ILogger<StineStudentReader> logger)
        {
            _ExamFilePath = examFilePath ?? throw new ArgumentNullException(nameof(examFilePath));
            _CourseFilePath = courseFilePath ?? throw new ArgumentNullException(nameof(courseFilePath));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (!File.Exists(_ExamFilePath)) throw new FileNotFoundException("Cannot find student exam list file.");
            if (!File.Exists(_CourseFilePath)) throw new FileNotFoundException("Cannot find student course list file.");
        }
        public IEnumerable<Student> Read()
        {
            List<Student> students = ReadStudents();
            _Logger.LogInformation($"Found {students.Count} students enrolled for the exam.");
            AddStudentInfo(students);
            return students;
        }

        private void AddStudentInfo(List<Student> students)
        {
            FileStream fileStream = new FileStream(_CourseFilePath, FileMode.Open);
            int counter = 0;
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] args = line.Split('\t');
                    Student student = students.SingleOrDefault(x => x.StudentId == Convert.ToInt32(args[1]));
                    if (student != null)
                    {
                        counter++;
                        student.FirstName = args[3];
                        student.LastName = args[2];
                        student.DegreeCourse = args[4];
                        student.Email = args[6];
                    }
                }
            }
            _Logger.LogInformation($"Found detailed information of {counter}/{students.Count} students.");
        }

        private List<Student> ReadStudents()
        {
            List<Student> students = new List<Student>();

            FileStream fileStream = new FileStream(_ExamFilePath, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                //Skip header
                string line = reader.ReadLine();

                while ((line = reader.ReadLine()) != null)
                {
                    string[] args = line.Split(';');
                    Student student = new Student()
                    {
                        StudentId = Convert.ToInt32(args[1].Replace("\"", ""))
                    };
                    if (students.Any(x => x.StudentId == student.StudentId))
                    {
                        throw new Exception("Duplicate student id");
                    }
                    students.Add(student);
                }
            }

            return students;
        }
    }
}