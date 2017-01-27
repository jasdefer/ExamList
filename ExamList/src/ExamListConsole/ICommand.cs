using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListConsole
{
    public interface ICommand
    {
        List<IArgument> Arguments { get; }
        void Run(string[] args);
    }
}
