namespace ExamListCore.Model
{
    public class Settings
    {
        public bool ReadBonusPoints { get; set; }
        public string ExamListPath { get; set; }
        public string CourseListPath { get; set; }
        public string RoomListPath { get; set; }
        public string BonusPointPath { get; set; }
        public double[] BonusPointLevels { get; set; }
        public double[] BonusPoints { get; set; }
        public double MaxBonusPoints { get; set; }
        public string LatexPath { get; set; }
        public int GroupCount { get; set; }
        public int RandomSeed { get; set; }
        public string AssignedStudentRooms { get; set; }
    }
}