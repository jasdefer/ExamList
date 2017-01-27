using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListConsole.Helper
{
    public static class InputHelper
    {
        /// <summary>
        /// Ask the user for an integer
        /// </summary>
        /// <param name="message">The message displayed to the user, before he enters his input</param>
        /// <param name="min">The minimum value accepted</param>
        /// <param name="max">The maximum value accepted</param>
        public static int AskInt(string message, int min=0, int max = int.MaxValue)
        {
            int number = 0;
            bool success = false;
            do
            {
                Console.WriteLine(message);
                success = int.TryParse(Console.ReadLine(), out number);
                if(success &&(number<min || number > max))
                {
                    success = false;
                }
            } while (!success);

            return number;
        }

        public static bool AreYouSure(string extraMessage)
        {
            do
            {
                Console.WriteLine("Are you sure (y/n)? " + extraMessage);
                ConsoleKeyInfo key = Console.ReadKey();
                Console.Write("\n");
                if (key.Key == ConsoleKey.Y)
                {
                    return true;
                }
                else if (key.Key == ConsoleKey.N)
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
                
            } while (true);
        }
    }
}
