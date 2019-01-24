using ExamListCore.Interfaces;
using ExamListCore.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExamListCore.Implementations
{
    public class RoomReader : IRoomReader
    {
        private readonly Settings settings;
        private readonly ILogger<RoomReader> logger;

        public RoomReader(ILogger<RoomReader> logger,
            IOptions<Settings> options)
        {
            if (options?.Value == null) throw new ArgumentNullException(nameof(options));
            settings = options.Value;
            this.logger = logger;
        }

        public IEnumerable<Room> ReadRooms()
        {
            if (!File.Exists(settings.RoomListPath)) throw new FileNotFoundException("Room list file not found.");

            var rooms = new List<Room>();
            var lines = File.ReadAllLines(settings.RoomListPath);
            for (int i = 0; i < lines.Length; i++)
            {
                var args = lines[i].Split('\t');
                if (args.Length != 2)
                {
                    logger.LogWarning("Invalid room list entry: " + lines[i]);
                }
                else
                {
                    bool success = int.TryParse(args[1], out int capacity);
                    if (!success)
                    {
                        logger.LogWarning($"Invalid room capacity: " + args[1]);
                    }
                    else
                    {
                        rooms.Add(new Room(args[0], capacity));
                    }
                }
            }
            logger.LogInformation($"Found {rooms.Count} rooms.");

            return rooms;
        }
    }
}