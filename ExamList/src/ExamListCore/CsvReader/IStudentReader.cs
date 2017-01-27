using ExamListCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListCore.CsvReader
{
    /// <summary>
    /// Defines, how a line of a csv file should be converted into a student
    /// </summary>
    public interface IStudentReader
    {
        bool ThrowExceptionOnInvalidStudent { get; set; }
        Student Read(string[] arguments);
    }
}
