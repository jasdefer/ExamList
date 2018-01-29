using ExamList.Interfaces;
using ExamList.Model;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace ExamList.Implementations
{
    /// <summary>
    /// Add bonus points to students. Read the point fraction from the Klausurtrainer from a file like:
    /// [Student_E_Mail]\t[Klausurtrainer_Points]\t[Point_Fraction]
    /// The e-mail is used to indentify the student and the point fraction is used to calculate the amount of bonus points
    /// </summary>
    public class KlausurtrainerReader : IBonusPointReader
    {
        private readonly string _FilePath;
        private readonly ILogger<KlausurtrainerReader> _Logger;

        public KlausurtrainerReader(string filePath, ILogger<KlausurtrainerReader> logger)
        {
            _FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (!File.Exists(filePath)) throw new FileNotFoundException("Cannot find the klausurtrainer file.");
        }

        public int StudentWithBonuspointRecord { get; set; }

        public decimal Read(Student student)
        {
            //Validation
            if (student == null) throw new ArgumentNullException(nameof(student));

            //If no e-mail is provided, the student cannot be identified. He gets no bonus points.
            if (string.IsNullOrEmpty(student.Email)) return 0;

            //Read the Klausurtrainer file and search it for the given student
            FileStream fileStream = new FileStream(_FilePath, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    string[] args = line.Split('\t');
                    if (args[0].ToUpperInvariant() == student.Email.ToUpperInvariant())
                    {
                        //Read the point fraction of the student in the Klausurtrainer
                        decimal fraction = Convert.ToDecimal(args[2].Replace("%",""))/100;
                        if (fraction < 0 || fraction > 1) throw new Exception("Unexpected point fraction.");
                        StudentWithBonuspointRecord++;

                        //Calculate the amount of bonus points
                        return FractionToBonusPoints(fraction);
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Calculates the amount of bonus points for a given fraction of points in the Klausurtrainer
        /// </summary>
        private decimal FractionToBonusPoints(decimal fraction)
        {
            if (fraction < 0.15m) return 0;
            else if (fraction < 0.25m) return 0.5m;
            else if (fraction < 0.35m) return 1m;
            else if (fraction < 0.45m) return 1.5m;
            else if (fraction < 0.55m) return 2m;
            else if (fraction < 0.65m) return 2.5m;
            else if (fraction < 0.75m) return 3m;
            else if (fraction < 0.85m) return 3.5m;
            else if (fraction < 0.95m) return 4m;
            else return 4.5m;
        }
    }
}