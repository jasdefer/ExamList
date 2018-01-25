using ExamList.Model;

namespace ExamList.Interfaces
{
    public interface IBonusPointReader
    {
        decimal Read(Student student);
        int StudentWithBonuspointRecord { get; set; }
    }
}
