using ExamListCore.Interfaces;
using ExamListCore.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExamListCore.Implementations
{
    public class KlausurtrainerPointReader : IBonusPointReader
    {
        private readonly Settings settings;
        private readonly ILogger<StineStudentReader> logger;

        public KlausurtrainerPointReader(IOptions<Settings> options,
            ILogger<StineStudentReader> logger)
        {
            if (options?.Value == null) throw new ArgumentNullException(nameof(options));
            settings = options.Value;
            this.logger = logger;
        }

        public void ReadBonusPoints(IEnumerable<Student> students)
        {
            ValidateBonusPointLevels();
            var onlyKlausurtainerStudents = new List<string>();
            int matchCount = 0;
            if (File.Exists(settings.BonusPointPath))
            {
                var lines = File.ReadAllLines(settings.BonusPointPath);
                for (int i = 0; i < lines.Length - 1; i+=2)
                {
                    var student = students.SingleOrDefault(x => x.Email == lines[i].ToLowerInvariant());
                    if (student != null)
                    {
                        var args = lines[i + 1].Split('\t');
                        bool success = int.TryParse(args[0], out int klausurtrainerPoints);
                        if (!success)
                        {
                            student.BonusPoints = GetBonusPoints(klausurtrainerPoints);
                            matchCount++;
                        }
                        else
                        {
                            logger.LogWarning("Invalid Klausurtrainer points: " + lines[i + 1]);
                        }
                    }
                    else
                    {
                        onlyKlausurtainerStudents.Add(lines[i]);
                    }
                }
            }
            else
            {
                logger.LogWarning("Klausurtrainer bonus point file not found.");
            }

            logger.LogInformation($"Added bonus points to {matchCount} students. {onlyKlausurtainerStudents.Count} students are registered for the Klausurtrainer but not for the exam.");
        }

        private void ValidateBonusPointLevels()
        {
            if (settings.BonusPointLevels.GetLength(0) != 2)
            {
                logger.LogWarning("Invalid bonus point levels.");
            }
            for (int i = 0; i < settings.BonusPointLevels.GetLength(1); i++)
            {
                var level = settings.BonusPointLevels[0, i];
                var points = settings.BonusPointLevels[1, i];
                if (level < 0 || level > 1) logger.LogWarning("The bonus point level is not between 0 and 100%.");
                if (i > 0 && level < settings.BonusPointLevels[0, i - 1])
                {
                    logger.LogWarning("The bonus point level is not increasing.");
                }

                if (i > 0 && points < settings.BonusPointLevels[1, i - 1])
                {
                    logger.LogWarning("The bonus points are not increasing.");
                }
            }
        }

        private double GetBonusPoints(double klausurtrainerPoints)
        {
            klausurtrainerPoints = Math.Min(klausurtrainerPoints, settings.MaxBonusPoints);
            var fraction = klausurtrainerPoints / settings.MaxBonusPoints;
            double bonus = 0;
            for (int i = 0; i < settings.BonusPointLevels.GetLongLength(1); i++)
            {
                var level = settings.BonusPointLevels[0, i];
                var points = settings.BonusPointLevels[1, i];
                if (fraction >= level) bonus = points;
            }
            return bonus;
        }
    }
}