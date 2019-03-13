# ExamList
Distribute students to rooms for an exam.
Just add a `app-settings.json` file to the folder of the console application.
The file should look like:
```json
{
  "ReadBonusPoints": true,
  "ExamListPath": "C:\\Users\\..\\ExamListPath.txt",
  "CourseListPath": "C:\\Users\\..\\CourseListPath.txt",
  "RoomListPath": "C:\\Users\\..\\RoomListPath.txt",
  "BonusPointPath": "C:\\Users\\..\\BonusPointPath.txt",
  "AssignedStudentRooms": "C:\\Users\\..\\AssignedStudentRoomsPath.txt",
  "SeatTablePath": "C:\\Users\\..\\SeatTablePath.txt",
  "MaxBonusPoints": 100,
  "LatexPath": "C:\\Users\\..\\CourseListPath.txt",
  "GroupCount": 2,
  "RandomSeed": 1,
  "BonusPointLevels": [ 0.25, 0.5, 0.75 ],
  "BonusPoints": [ 1, 2, 3 ]
}
```

1. ReadBonusPoints (true/false)
2. ExamListPath: Path to the file with the list of students enroled for the exam
3. CourseListPath: Path to the file with a list of detailed information for the students
4. RoomListPath: Path to the file with the list of rooms
5. BonusPointPath: Path to the file with the Klausurtrainer Points of the students
6. AssignedStudentRooms: Path to the file with a list of students, which are placed in certain rooms, instead of randomly assignments
7. SeatTablePath: This file will be created and contains a list with the seat numbers and the room for each student
8. MaxBonusPoints: Maximum amount of bonus points
9. LatexPath: This file will be created and can be used for a latex template
10. GroupCount: Number of groups for the exam
11. RandomSeed: Seed for randomizing the student room distribution
12. BonusPointLevels: Percentage levels where students get bonus points
13. BonusPoints: The amount of bonus points for each corresponding bonus point level
