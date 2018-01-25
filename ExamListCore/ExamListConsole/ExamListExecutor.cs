using ExamList;
using System;

namespace ExamListConsole
{
    public class ExamListExecutor : IExamListTask
    {
        private readonly ExamListManager _Manager;

        public ExamListExecutor(ExamListManager manager)
        {
            _Manager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        public void Run()
        {
            _Manager.Start(true);
            Console.ReadLine();
        }
    }
}