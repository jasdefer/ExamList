using ExamList.Interfaces;
using ExamList.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamList.Implementations
{
    public class StineStudentReader : IStudentReader
    {
        private readonly string _ExamFilePath;
        private readonly string _CourseFilePath;

        public StineStudentReader(string examFilePath, string courseFilePath)
        {
            _ExamFilePath = examFilePath ?? throw new ArgumentNullException(nameof(examFilePath));
            _CourseFilePath = courseFilePath ?? throw new ArgumentNullException(nameof(courseFilePath));

            if (!File.Exists(_ExamFilePath)) throw new FileNotFoundException("Cannot find student exam list file.");
            if (!File.Exists(_CourseFilePath)) throw new FileNotFoundException("Cannot find student course list file.");
        }
        public IEnumerable<Student> Read()
        {
            List<Student> students = ReadStudents();
            AddStudentInfo(students);
            return students;
        }

        private void AddStudentInfo(List<Student> students)
        {
            FileStream fileStream = new FileStream(_CourseFilePath, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] args = line.Split('\t');
                    Student student = students.SingleOrDefault(x => x.StudentId == Convert.ToInt32(args[1]));
                    if (student != null)
                    {
                        student.FirstName = args[3];
                        student.LastName = args[2];
                        student.DegreeCourse = args[4];
                        student.Email = args[6];
                    }
                }
            }
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
