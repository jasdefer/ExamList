using ExamList.Interfaces;
using ExamList.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExamList.Implementations
{
    public class DefaultRoomReader : IRoomReader
    {
        private readonly string _RoomFilePath;

        public DefaultRoomReader(string roomFilePath)
        {
            _RoomFilePath = roomFilePath ?? throw new ArgumentNullException(nameof(roomFilePath));
            if (!File.Exists(_RoomFilePath)) throw new FileNotFoundException("Cannot find room file.");
        }
        public IEnumerable<Room> Read()
        {
            List<Room> rooms = new List<Room>();
            FileStream fileStream = new FileStream(_RoomFilePath, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    string[] args = line.Split('\t');
                    Room room = new Room(args[0], Convert.ToInt32(args[1]));
                }
            }

            return rooms;
        }
    }
}