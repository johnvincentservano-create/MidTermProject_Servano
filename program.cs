using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;

namespace StudentGradeProgram
{
    public class Subject
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }

    public class Grade
    {
        public string? SubjectId { get; set; }
        public decimal Score { get; set; }
    }

    public class Student
    {
        public string? FirstName { get; set; }
        public string? MiddleInitial { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string? Address { get; set; }
        public string? ContactNumber { get; set; }
        public string? Course { get; set; }
        public int YearLevel { get; set; }
        public List<Subject> Subjects { get; set; } = new List<Subject>();
        public List<Grade> Grades { get; set; } = new List<Grade>();
    }

    class Program
    {
        static List<Student> allStudents = new List<Student>();

        static string filePath = "C:\\Users\\sxoka\\Documents\\Csharp\\MidTermProject_Servano\\MidTermProject_Servano\\Students.txt";

        static void Main()
        {
            while (true)
            {
                LoadStudents();

                MainMenu();
                Console.Write("Select an option: ");

                string input = Console.ReadLine() ?? string.Empty;
                int selectedOption;

                if (!int.TryParse(input, out selectedOption) || selectedOption < 1 || selectedOption > 5)
                {
                    Console.WriteLine("Please enter a number between 1 and 5.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    continue;
                }

                if (selectedOption == 5)
                {
                    Console.WriteLine("Thank you! Goodbye.");
                    break;
                }

                switch (selectedOption)
                {
                    case 1:
                        RegisterNewStudent();
                        break;
                    case 2:
                        EnrollStudentInSubjects();
                        break;
                    case 3:
                        EnterStudentGrades();
                        break;
                    case 4:
                        ShowStudentGrades();
                        break;
                    case 5:
                        return;
                }

                SaveStudents();

                Console.WriteLine("\nPress any key to return to main menu...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void LoadStudents()
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                var loaded = JsonSerializer.Deserialize<List<Student>>(json);
                if (loaded != null)
                {
                    allStudents = loaded;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void SaveStudents()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(allStudents, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("╔═════════════════════════════╗");
            Console.WriteLine("║          MAIN MENU          ║");
            Console.WriteLine("╠═════════════════════════════╣");
            Console.WriteLine("║ 1. Register Student         ║");
            Console.WriteLine("║ 2. Enroll Student Subjects  ║");
            Console.WriteLine("║ 3. Enter Grades             ║");
            Console.WriteLine("║ 4. Show Grades by Student   ║");
            Console.WriteLine("║ 5. Exit                     ║");
            Console.WriteLine("╚═════════════════════════════╝");
        }


        static void RegisterNewStudent()
        {
            Console.Clear();
            string firstName = string.Empty;
            string middleInitial = string.Empty;
            string lastName = string.Empty;
            DateTime birthDate = default;
            string birthDateString = string.Empty;
            string address = string.Empty;
            string contactNumber = string.Empty;
            string course = string.Empty;
            int yearLevel = 0;

            string dateFormat = "MM/dd/yyyy";

            Console.WriteLine("\n--- Register New Student ---");

            while (true)
            {
                Console.Write("First Name: ");
                firstName = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(firstName))
                {
                    Console.WriteLine("First name cannot be empty. Try again.");
                    continue;
                }

                if (!firstName.ToCharArray().All(char.IsLetter))
                {
                    Console.WriteLine("First name should contain letters only (no numbers or symbols). Try again.");
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.Write("Middle Initial (one letter): ");
                middleInitial = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(middleInitial))
                {
                    Console.WriteLine("Middle initial cannot be empty. Try again.");
                    continue;
                }

                if (middleInitial.Length != 1 || !char.IsLetter(middleInitial[0]))
                {
                    Console.WriteLine("Middle initial must be exactly one letter. Try again.");
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.Write("Last Name: ");
                lastName = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(lastName))
                {
                    Console.WriteLine("Last name cannot be empty. Try again.");
                    continue;
                }

                if (!lastName.ToCharArray().All(char.IsLetter))
                {
                    Console.WriteLine("Last name should contain letters only (no numbers or symbols). Try again.");
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.Write("Birth Date (MM/dd/yyyy): ");
                birthDateString = Console.ReadLine() ?? string.Empty;

                if (DateTime.TryParseExact(birthDateString, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
                {
                    break;
                }

                Console.WriteLine("Invalid date format. Please use MM/dd/yyyy (example: 05/14/2003). Try again.");
            }

            int calculatedAge = DateTime.Today.Year - birthDate.Year;
            if (DateTime.Today.Month < birthDate.Month ||
               (DateTime.Today.Month == birthDate.Month && DateTime.Today.Day < birthDate.Day))
            {
                calculatedAge--;
            }

            while (true)
            {
                Console.Write("Address: ");
                address = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(address))
                {
                    Console.WriteLine("Address cannot be empty. Try again.");
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.Write("Contact Number: ");
                contactNumber = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(contactNumber))
                {
                    Console.WriteLine("Contact number cannot be empty. Try again.");
                    continue;
                }

                string digitsOnly = new string(contactNumber.Where(char.IsDigit).ToArray());
                if (digitsOnly.Length < 7)
                {
                    Console.WriteLine("Contact number should contain at least 7 digits. Try again.");
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.Write("Course (e.g. BSIT, BSME): ");
                course = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(course))
                {
                    Console.WriteLine("Course cannot be empty. Try again.");
                    continue;
                }

                break;
            }

            while (true)
            {
                Console.Write("Year Level (1-4): ");
                string yearInput = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(yearInput, out yearLevel) && yearLevel >= 1 && yearLevel <= 4)
                {
                    break;
                }

                Console.WriteLine("Please enter a number between 1 and 4. Try again.");
            }

            Student newStudent = new Student
            {
                FirstName = firstName,
                MiddleInitial = middleInitial,
                LastName = lastName,
                BirthDate = birthDate,
                Age = calculatedAge,
                Address = address,
                ContactNumber = contactNumber,
                Course = course,
                YearLevel = yearLevel
            };

            allStudents.Add(newStudent);

            Console.WriteLine($"Student {firstName} {lastName} registered successfully!");
        }

        static void EnrollStudentInSubjects()
        {
            Console.Clear();
            if (allStudents.Count == 0)
            {
                Console.WriteLine("No students registered yet. Please register a student first.");
                return;
            }

            Console.WriteLine("\n--- Enroll Student in Subjects ---");
            Console.WriteLine("Select a student:");

            for (int i = 0; i < allStudents.Count; i++)
            {
                Student s = allStudents[i];
                string fullName = $"{s.FirstName} {s.MiddleInitial}. {s.LastName}";
                Console.WriteLine($"{i + 1}. {fullName,-25} ({s.Course} - Year {s.YearLevel})");
            }

            Console.Write("Enter student number: ");
            int selectedNumber;
            if (!int.TryParse(Console.ReadLine(), out selectedNumber) || selectedNumber < 1 || selectedNumber > allStudents.Count)
            {
                Console.WriteLine("Invalid student selection.");
                return;
            }

            Student selectedStudent = allStudents[selectedNumber - 1];

            List<Subject> availableSubjects = new List<Subject>();

            if (selectedStudent.Course?.ToUpper() == "BSIT")
            {
                availableSubjects.Add(new Subject { Id = "COMP 102IT", Name = "Computer Applications" });
                availableSubjects.Add(new Subject { Id = "IT 104A", Name = "Computer Programming 2, Lec." });
                availableSubjects.Add(new Subject { Id = "IT 104B", Name = "Computer Programming 2, Lab." });
                availableSubjects.Add(new Subject { Id = "IT 105A", Name = "Platform Technologies" });
                availableSubjects.Add(new Subject { Id = "IT 106A", Name = "IT Social and Professional Issues" });
                availableSubjects.Add(new Subject { Id = "GEC 103A", Name = "Understanding the Self" });
                availableSubjects.Add(new Subject { Id = "GEC 104A", Name = "Arts Appreciation" });
                availableSubjects.Add(new Subject { Id = "RZL 104A", Name = "Life and Works of Rizal" });
                availableSubjects.Add(new Subject { Id = "PE 102A", Name = "Physical Education 2" });
                availableSubjects.Add(new Subject { Id = "THEO 102A", Name = "Scriptures, the Sacraments and the Liturgy" });
            }
            else
            {
                Console.WriteLine($"No subjects have been established for course: {selectedStudent.Course}");
                return;
            }

            Console.WriteLine($"\nAvailable subjects for {selectedStudent.Course}:");
            for (int i = 0; i < availableSubjects.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableSubjects[i].Id,-12} {availableSubjects[i].Name}");
            }

            Console.WriteLine("\nEnter subject numbers to enroll (separate with space, e.g. 1 3 5 8), then press Enter:");
            string inputLine = Console.ReadLine() ?? string.Empty;
            string[] parts = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int addedCount = 0;

            foreach (string part in parts)
            {
                if (int.TryParse(part, out int num) && num >= 1 && num <= availableSubjects.Count)
                {
                    Subject chosenSubject = availableSubjects[num - 1];
                    bool alreadyEnrolled = false;

                    foreach (Subject enrolled in selectedStudent.Subjects)
                    {
                        if (enrolled.Id == chosenSubject.Id)
                        {
                            alreadyEnrolled = true;
                            break;
                        }
                    }

                    if (!alreadyEnrolled)
                    {
                        selectedStudent.Subjects.Add(chosenSubject);
                        selectedStudent.Grades.Add(new Grade { SubjectId = chosenSubject.Id, Score = 0 });
                        Console.WriteLine($"Added: {chosenSubject.Id} - {chosenSubject.Name}");
                        addedCount++;
                    }
                    else
                    {
                        Console.WriteLine($"Already enrolled: {chosenSubject.Id}");
                    }
                }
            }

            if (addedCount > 0)
            {
                Console.WriteLine($"\n{addedCount} new subject(s) enrolled successfully.");
            }
            else
            {
                Console.WriteLine("\nNo new subjects were enrolled.");
            }
        }

        static void EnterStudentGrades()
        {
            Console.Clear();

            if (allStudents.Count == 0)
            {
                Console.WriteLine("No students registered yet.");
                return;
            }

            Console.WriteLine("\n--- Enter Grades ---");
            for (int i = 0; i < allStudents.Count; i++)
            {
                Student s = allStudents[i];
                Console.WriteLine($"{i + 1}. {s.FirstName} {s.LastName} ({s.Course})");
            }

            Console.Write("Select student number: ");
            int selectedNumber;
            if (!int.TryParse(Console.ReadLine(), out selectedNumber) || selectedNumber < 1 || selectedNumber > allStudents.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            Student selectedStudent = allStudents[selectedNumber - 1];

            if (selectedStudent.Subjects.Count == 0)
            {
                Console.WriteLine("This student is not enrolled in any subjects yet.");
                return;
            }

            Console.WriteLine($"\nEntering grades for {selectedStudent.FirstName} {selectedStudent.LastName}");

            for (int i = 0; i < selectedStudent.Subjects.Count; i++)
            {
                Subject sub = selectedStudent.Subjects[i];
                Console.Write($"{sub.Id,-12} {sub.Name,-35}: ");

                decimal gradeValue;
                while (!decimal.TryParse(Console.ReadLine(), out gradeValue) || gradeValue < 0 || gradeValue > 100)
                {
                    Console.Write("Please enter a number between 0 and 100: ");
                }

                selectedStudent.Grades[i].Score = gradeValue;
            }

            Console.WriteLine("All grades have been saved successfully.");
        }

        static void ShowStudentGrades()
        {
            Console.Clear();
            if (allStudents.Count == 0)
            {
                Console.WriteLine("No students registered yet.");
                return;
            }

            Console.WriteLine("\n--- View Student Grades ---");
            for (int i = 0; i < allStudents.Count; i++)
            {
                Student s = allStudents[i];
                Console.WriteLine($"{i + 1}. {s.FirstName} {s.LastName} ({s.Course})");
            }

            Console.Write("Select student number: ");
            int selectedNumber;
            if (!int.TryParse(Console.ReadLine(), out selectedNumber) || selectedNumber < 1 || selectedNumber > allStudents.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            Student selectedStudent = allStudents[selectedNumber - 1];

            if (selectedStudent.Subjects.Count == 0)
            {
                Console.WriteLine("No subjects enrolled yet.");
                return;
            }

            Console.WriteLine($"\nGrades for {selectedStudent.FirstName} {selectedStudent.LastName}");
            Console.WriteLine("──────────────────────────────────────────────────────────────");

            decimal total = 0;
            int count = 0;

            for (int i = 0; i < selectedStudent.Subjects.Count; i++)
            {
                Subject sub = selectedStudent.Subjects[i];
                decimal grade = selectedStudent.Grades[i].Score;

                Console.WriteLine($"{sub.Id,-12} {sub.Name,-35} : {grade,5}");
                total += grade;
                count++;
            }

            if (count > 0)
            {
                decimal average = total / count;
                Console.WriteLine($"\nAverage Grade: {average:F2}");
            }
        }
    }
}
[
  {
    "FirstName": "JohnVincent",
    "MiddleInitial": "M",
    "LastName": "Servano",
    "BirthDate": "2007-04-30T00:00:00",
    "Age": 18,
    "Address": "brgy.toong",
    "ContactNumber": "09096241732",
    "Course": "bsit",
    "YearLevel": 1,
    "Subjects": [
      {
        "Id": "COMP 102IT",
        "Name": "Computer Applications"
      },
      {
        "Id": "IT 104A",
        "Name": "Computer Programming 2, Lec."
      },
      {
        "Id": "IT 104B",
        "Name": "Computer Programming 2, Lab."
      },
      {
        "Id": "IT 105A",
        "Name": "Platform Technologies"
      },
      {
        "Id": "IT 106A",
        "Name": "IT Social and Professional Issues"
      },
      {
        "Id": "GEC 103A",
        "Name": "Understanding the Self"
      },
      {
        "Id": "GEC 104A",
        "Name": "Arts Appreciation"
      },
      {
        "Id": "RZL 104A",
        "Name": "Life and Works of Rizal"
      },
      {
        "Id": "PE 102A",
        "Name": "Physical Education 2"
      },
      {
        "Id": "THEO 102A",
        "Name": "Scriptures, the Sacraments and the Liturgy"
      }
    ],
    "Grades": [
      {
        "SubjectId": "COMP 102IT",
        "Score": 100
      },
      {
        "SubjectId": "IT 104A",
        "Score": 100
      },
      {
        "SubjectId": "IT 104B",
        "Score": 100
      },
      {
        "SubjectId": "IT 105A",
        "Score": 100
      },
      {
        "SubjectId": "IT 106A",
        "Score": 100
      },
      {
        "SubjectId": "GEC 103A",
        "Score": 100
      },
      {
        "SubjectId": "GEC 104A",
        "Score": 100
      },
      {
        "SubjectId": "RZL 104A",
        "Score": 100
      },
      {
        "SubjectId": "PE 102A",
        "Score": 100
      },
      {
        "SubjectId": "THEO 102A",
        "Score": 100
      }
    ]
  }
]    
