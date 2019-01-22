using ExamListCore.Interfaces;
using ExamListCore.Model;
using Microsoft.Extensions.Logging;

namespace ExamListCore
{
    public class ExamListManager
    {
        private readonly ILogger logger;
        private readonly Settings settings;
        private readonly IStudentReader studentReader;
        private readonly IRoomReader roomReader;
        private readonly IBonusPointReader bonusPointReader;
        private readonly IExamListPrinter examListPrinter;
        private readonly IStudentDistribution studentDistribution;

        public ExamListManager(ILogger logger,
            Settings settings,
            IStudentReader studentReader,
            IRoomReader roomReader,
            IBonusPointReader bonusPointReader,
            IExamListPrinter examListPrinter,
            IStudentDistribution studentDistribution)
        {
            this.logger = logger;
            this.settings = settings;
            this.studentReader = studentReader;
            this.roomReader = roomReader;
            this.bonusPointReader = bonusPointReader;
            this.examListPrinter = examListPrinter;
            this.studentDistribution = studentDistribution;
        }

        public void DistributeStudents()
        {
            var students = studentReader.ReadStudents();
            var rooms = roomReader.ReadRooms();
            if (settings.ReadBonusPoints)
            {
                bonusPointReader.ReadBonusPoints(students);
            }
            studentDistribution.Distribute(students, rooms);
            examListPrinter.Print(students);
        }
    }
}