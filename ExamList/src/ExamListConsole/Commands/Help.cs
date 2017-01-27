using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListConsole.Commands
{
    public class Help : ICommand
    {
        public Dictionary<string,ICommand> Commands { get; set; }
        List<IArgument> ICommand.Arguments
        {
            get
            {
                return null;
            }
        }

        public void Run(string[] args)
        {
            Console.WriteLine("To execute an command type its key followed by arguments separated by spaces. If an arugment needs an input, write it directly behind the argument, but separated by a space as well.");
            foreach (var command in Commands)
            {
                if (this != command.Value)
                {
                    Console.WriteLine($"{command.Key}:");
                    foreach (var argument in command.Value.Arguments)
                    {
                        Console.WriteLine($"\t-{string.Join(";", argument.Keys)}: {argument.Description}");
                    }
                }
            }
        }
    }
}
