using ExamListCore;
using Microsoft.Extensions.Logging;

namespace ExamListConsole
{
    public class App
    {
        private readonly ExamListManager examListManager;
        private readonly ILogger<ExamListManager> logger;

        public App(ExamListManager examListManager,
            ILogger<ExamListManager> logger)
        {
            this.examListManager = examListManager;
            this.logger = logger;
        }

        public void Run()
        {
            logger.LogInformation("Welcome to the exam list manager.");
            examListManager.DistributeStudents();
        }
    }
}