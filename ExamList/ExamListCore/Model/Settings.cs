﻿namespace ExamListCore.Model
{
    public class Settings
    {
        public bool ReadBonusPoints { get; set; }
        public string ExamListPath { get; set; }
        public string CourseListPath { get; set; }
        public string RoomListPath { get; set; }
        public string BonusPointPath { get; set; }
        public double[,] BonusPointLevels { get; set; }
        public double MaxBonusPoints { get; set; }
    }
}