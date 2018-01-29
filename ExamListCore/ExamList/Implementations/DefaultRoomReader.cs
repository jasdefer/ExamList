using ExamList.Interfaces;
using ExamList.Model;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ExamList.Implementations
{
    /// <summary>
    /// Read the rooms from a file like:
    /// Audimax\t200
    /// ESA-A\t150
    /// ...
    /// </summary>
    public class DefaultRoomReader : IRoomReader
    {
        private readonly string _RoomFilePath;
        private readonly ILogger<DefaultRoomReader> _Logger;

        public DefaultRoomReader(string roomFilePath, ILogger<DefaultRoomReader> logger)
        {
            _RoomFilePath = roomFilePath ?? throw new ArgumentNullException(nameof(roomFilePath));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                    if (args.Length == 3)
                    {
                        room.Offset = Convert.ToInt32(args[2]);
                    }
                    rooms.Add(room);
                    _Logger.LogInformation("Added Room " + room.ToString());
                }
            }

            _Logger.LogInformation($"Added {rooms.Count} rooms.");
            return rooms;
        }
    }
}