﻿namespace ExamList.Model
{
    public class Room
    {
        public Room(string name, int capacity)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            Capacity = capacity;
        }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Offset { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Capacity})";
        }
    }
}