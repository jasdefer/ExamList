using ExamListCore;

namespace ExamListConsole
{
    public class App
    {
        private readonly ExamListManager examListManager;

        public App(ExamListManager examListManager)
        {
            this.examListManager = examListManager;
        }

        public void Run()
        {
            examListManager.DistributeStudents();
        }
    }
}