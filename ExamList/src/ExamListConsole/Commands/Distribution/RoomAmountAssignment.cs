using ExamListConsole.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListConsole.Commands.Distribution
{
    public class RoomAmountAssignment : IAssignment
    {
        public int Amount { get; private set; }
        private readonly string[] _Keys = new string[] { "room", "rooms","r" };
        private bool _Valid = true;
        public string Description
        {
            get
            {
                return "Set the amount of rooms, available for the students";
            }
        }

        public string[] Keys
        {
            get
            {
                return _Keys;
            }
        }

        public bool Valid
        {
            get
            {
                return _Valid;
            }
        }

        public void Assign(string argument)
        {
            int amount = 0;
            bool success = int.TryParse(argument, out amount);
            if (success && amount > 0)
            {
                Amount = amount;
            }else
            {
                Console.WriteLine("Invalid amount of rooms");
                _Valid = false;
            }
        }

        public void Reassign()
        {
            Console.WriteLine($"Room amount not set: {Description}");
            Amount = InputHelper.AskInt("Please enter a valid amount of rooms: ", 1);
        }
    }
}
