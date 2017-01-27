using ExamListConsole.Commands;
using ExamListConsole.Commands.Distribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamListConsole
{
    public class Program
    {
        public static bool RUN = true;
        public static readonly Dictionary<string, ICommand> _COMMANDS = new Dictionary<string, ICommand>();


        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome");
            //Load all commands to the dictionary
            LoadCommands();
            do
            {
                //Read the input of the user
                Console.WriteLine("Waiting for a command...");
                string[] arguments = Console.ReadLine().ToLowerInvariant().Split(' ');
                //Check if a corresponding command exists
                if (_COMMANDS.ContainsKey(arguments[0]))
                {
                    ICommand command = _COMMANDS[arguments[0]];
                    //Prepare the command with the given arguments
                    RunArguments(command, arguments);
                    //Check if any invalid arguments where passed
                    if(command.Arguments==null || !command.Arguments.OfType<IAssignment>().Any(x => !x.Valid))
                    {
                        //Run the command
                        command.Run(arguments);
                    }
                    else
                    {
                        //At least one argument for an assignment was invalid
                        Console.WriteLine("Invalid arguments. Cancelling command...");
                    }
                    
                }
                else
                {
                    Console.WriteLine("Invalid command. Type help to get some help :)");
                }
            } while (RUN);
        }

        private static void RunArguments(ICommand command, string[] args)
        {
            //Iterate over the arguments. Start at 1 because 0 was the key of the command
            for (int i = 1; i < args.Length; i++)
            {
                //Check if the argument exists
                IArgument argument = command.Arguments.SingleOrDefault(x => x.Keys.Contains(args[i]));
                if (argument!=null)
                {
                    //Check if the command is an assignment or statement
                    IAssignment assignment = argument as IAssignment;
                    if (assignment != null)
                    {
                        //Run the assignment. The argument after the assignment is the parameter
                        if (i >= args.Length - 1)
                        {
                            Console.WriteLine("No value given for " + args[i]);
                        }
                        //Run the assignment
                        assignment.Assign(args[i + 1]);
                        i++;
                    }
                    else
                    {
                        //Run the statement
                        IStatement statement = argument as IStatement;
                        if (statement != null)
                        {
                            statement.Set();
                        }
                        else
                        {
                            throw new Exception("The argument is neither a statement nor an assignemnt");
                        }
                    }

                }
                else
                {
                    //Cancel because argument does not exists
                    Console.WriteLine($"Invalid argument: {args[i]}");
                    return;
                }
            }
        }

        private static void LoadCommands()
        {
            Help help = new Help();
            _COMMANDS.Add("help", help);
            _COMMANDS.Add("dis", new DistributionCommand());
            //Check if the arguments of a command are unique
            foreach (ICommand command in _COMMANDS.Values)
            {
                List<string> keys = new List<string>();
                if (command.Arguments != null)
                {
                    foreach (var argument in command.Arguments)
                    {
                        keys.AddRange(argument.Keys);
                    }
                }
                if (keys.Count != keys.Distinct().Count())
                {
                    throw new Exception("Duplicate keys for arguments of a command");
                }
            }
            help.Commands = _COMMANDS;
        }
    }
}
