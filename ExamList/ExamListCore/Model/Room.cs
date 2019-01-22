using System;

namespace ExamListCore.Model
{
    public class Room
    {
        public Room(string name, int capacity)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity));
            Capacity = capacity;
        }

        public string Name { get; set; }
        public int Capacity { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Capacity})";
        }
    }
}