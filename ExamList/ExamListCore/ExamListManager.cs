using ExamListCore.Interfaces;
using ExamListCore.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        private readonly IRoomListPrinter roomListPrinter;

        public ExamListManager(ILogger<ExamListManager> logger,
            IOptions<Settings> options,
            IStudentReader studentReader,
            IRoomReader roomReader,
            IBonusPointReader bonusPointReader,
            IExamListPrinter examListPrinter,
            IStudentDistribution studentDistribution,
            IRoomListPrinter roomListPrinter)
        {
            this.logger = logger;
            this.settings = options.Value;
            this.studentReader = studentReader;
            this.roomReader = roomReader;
            this.bonusPointReader = bonusPointReader;
            this.examListPrinter = examListPrinter;
            this.studentDistribution = studentDistribution;
            this.roomListPrinter = roomListPrinter;
        }

        public void DistributeStudents()
        {
            var students = studentReader.ReadStudents();
            var rooms = roomReader.ReadRooms();
            if (settings.ReadBonusPoints)
            {
                bonusPointReader.ReadBonusPoints(students);
            }
            students = studentDistribution.Distribute(students, rooms);
            examListPrinter.Print(students);
            roomListPrinter.Print(students);
        }
    }
}