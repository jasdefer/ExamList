using ExamListCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamListCore
{
    public class Distributor
    {
        public List<Student> Students { get; set; }
        public List<Room> Rooms { get; private set; }

        /// <summary>
        /// Add a new room. The students may be distributed this room.
        /// </summary>
        /// <param name="name">The name of the room</param>
        /// <param name="capacity">The maximum amount of students allowed in this room</param>
        public void AddRoom(string name, int capacity)
        {
            Room room = new Room()
            {
                Name = name,
                Capacity = capacity
            };
            AddRoom(room);
        }

        /// <summary>
        /// Add a new room. The students may be distributed this room.
        /// </summary>
        /// <param name="room"></param>
        public void AddRoom(Room room)
        {
            if (Rooms == null)
            {
                Rooms = new List<Room>();
            }
            if(room == null || string.IsNullOrEmpty(room.Name) || room.Capacity < 1)
            {
                throw new ArgumentException("Invalid room");
            }
            Rooms.Add(room);
        }

        /// <summary>
        /// Distribute the students to rooms in a random order. The first rooms will be filled first
        /// </summary>
        public void Distribute(int seed)
        {
            //Validate all inputs
            if(Students == null || Students.Count < 1)
            {
                throw new ArgumentNullException("Students");
            }

            if (Rooms == null || Rooms.Count < 1)
            {
                throw new ArgumentNullException("Rooms");
            }

            if(Students.Count > Rooms.Sum(x => x.Capacity))
            {
                throw new InvalidOperationException($"Capacity exceeded. {Students.Count} students.");
            }


            Random rnd = new Random(seed);
            //Give an index to each student
            List<int> indicies = Enumerable.Range(0, Students.Count).ToList();
            foreach (Room room in Rooms)
            {
                int max = Math.Min(indicies.Count, room.Capacity);
                for (int i = 0; i < max; i++)
                {
                    //Get a random index from the list
                    int index = indicies[rnd.Next(0, indicies.Count)];
                    Students[index].SetSeat(room,i+1);
                    //Remove the index from the list to prevent him beeing drawn a second time
                    indicies.Remove(index);
                }
            }
            Students = Students.OrderBy(x => x.Room).OrderBy(x => x.Seat).ToList();
        }
    }
}
