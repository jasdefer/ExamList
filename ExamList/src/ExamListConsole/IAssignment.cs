using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListConsole
{
    public interface IAssignment : IArgument
    {
        void Assign(string argument);
        bool Valid { get; }
    }
}
