using ConsoleCommandHelperCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamListConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Center center = new Center();
            center.AddCommand<CreateLatexOutput>("exam");
            center.Run();
        }

    }
}
