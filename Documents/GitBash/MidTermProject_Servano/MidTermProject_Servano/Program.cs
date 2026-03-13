using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace StudentManagementSystem
{
    public class EnrolledSubject
    {
        public string SubjectID { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public double? Grade { get; set; } // null = not yet entered
    }

    public class Student
    {
        public string FirstName { get; set; } = string.Empty;
        public string MiddleInitial { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string Address { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public int Year { get; set; }
        public List<EnrolledSubject> EnrolledSubjects { get; set; } = new List<EnrolledSubject>();

        public void CalculateAge()
        {
            Age = DateTime.Today.Year - BirthDate.Year;
            if (BirthDate.Date > DateTime.Today.AddYears(-Age))
                Age--;
        }
    }

    class Program
    {
        private static List<Student> students = new List<Student>();
        private static readonly string dataFile = "C:\\Users\\sxoka\\Documents\\MidTerm_Project\\StudentList.txt";

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            LoadData();

            bool running = true;
            while (running)
            {
                Console.Clear();
                DrawMainMenu();

                Console.Write("Enter your choice (1-5): ");
                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        RegisterStudent();
                        SaveData();
                        break;

                    case "2":
                        EnrollSubjects();
                        SaveData();
                        break;

                    case "3":
                        EnterGrades();
                        SaveData();
                        break;

                    case "4":
                        ShowGrades();
                        break;

                    case "5":
                        SaveData();
                        running = false;
                        Console.WriteLine("\nThank you! Data saved to students_data.txt");
                        break;

                    default:
                        Console.WriteLine("\nInvalid choice! Please enter 1-5.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey(true);
                }
            }
        }

        private static void DrawMainMenu()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║           MIDTERM PROJECT - STUDENT SYSTEM         ║");
            Console.WriteLine("║                  MAIN MENU                         ║");
            Console.WriteLine("╠════════════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Register Student                               ║");
            Console.WriteLine("║  2. Enroll Student Subjects                        ║");
            Console.WriteLine("║  3. Enter Grades                                   ║");
            Console.WriteLine("║  4. Show Grade by Student                          ║");
            Console.WriteLine("║  5. Exit                                           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        private static void LoadData()
        {
            if (File.Exists(dataFile))
            {
                try
                {
                    string json = File.ReadAllText(dataFile);
                    students = JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
                }
                catch
                {
                    // If file is corrupted or empty → start with empty list
                    students = new List<Student>();
                }
            }
        }

        private static void SaveData()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(students, options);
                File.WriteAllText(dataFile, json);
                // Optional: show small confirmation
                // Console.WriteLine("Data saved to students_data.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        private static string GetNonEmptyInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(input)) return input;
                Console.WriteLine("→ This field cannot be empty. Please try again.");
            }
        }

        private static void RegisterStudent()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║             REGISTER NEW STUDENT           ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            var student = new Student();

            student.FirstName = GetNonEmptyInput("First Name          : ");
            student.MiddleInitial = GetNonEmptyInput("Middle Initial      : ");
            student.LastName = GetNonEmptyInput("Last Name           : ");

            while (true)
            {
                Console.Write("Birthdate (YYYY-MM-DD) : ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime bd) &&
                    bd < DateTime.Today && bd > DateTime.Today.AddYears(-150))
                {
                    student.BirthDate = bd;
                    student.CalculateAge();
                    Console.WriteLine($"→ Calculated Age: {student.Age}");
                    break;
                }
                Console.WriteLine("→ Invalid birthdate. Must be a valid past date.");
            }

            student.Address = GetNonEmptyInput("Address             : ");

            while (true)
            {
                Console.Write("Contact Number (09xxxxxxxxx) : ");
                string? cn = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(cn) && cn.Length == 11 &&
                    cn.All(char.IsDigit) && cn.StartsWith("09"))
                {
                    student.ContactNumber = cn;
                    break;
                }
                Console.WriteLine("→ Invalid format. Use 11 digits starting with 09.");
            }

            student.Course = GetNonEmptyInput("Course (e.g. BSIT)  : ");

            while (true)
            {
                Console.Write("Year (1-4)          : ");
                if (int.TryParse(Console.ReadLine(), out int y) && y >= 1 && y <= 4)
                {
                    student.Year = y;
                    break;
                }
                Console.WriteLine("→ Year must be between 1 and 4.");
            }

            students.Add(student);
            Console.WriteLine("\nStudent registered successfully!");
        }

        private static int? SelectStudent()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No students registered yet.");
                return null;
            }

            Console.WriteLine("\nRegistered Students:");
            Console.WriteLine("───────────────────────────────────────────────");
            for (int i = 0; i < students.Count; i++)
            {
                var s = students[i];
                Console.WriteLine($"{i + 1,2}. {s.LastName}, {s.FirstName} {s.MiddleInitial} - {s.Course} Yr {s.Year} (Age {s.Age})");
            }
            Console.WriteLine("───────────────────────────────────────────────");

            while (true)
            {
                Console.Write("Select student number: ");
                if (int.TryParse(Console.ReadLine(), out int num) && num >= 1 && num <= students.Count)
                    return num - 1;

                Console.WriteLine("→ Invalid selection.");
            }
        }

        private static void EnrollSubjects()
        {
            var idx = SelectStudent();
            if (!idx.HasValue) return;
            var student = students[idx.Value];

            Console.Clear();
            Console.WriteLine($"Enrolling subjects for: {student.LastName}, {student.FirstName}\n");

            bool more = true;
            while (more)
            {
                string sid = GetNonEmptyInput("Subject ID   : ");
                string name = GetNonEmptyInput("Subject Name : ");

                if (student.EnrolledSubjects.Any(s => s.SubjectID.Equals(sid, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("→ Subject ID already enrolled.");
                }
                else
                {
                    student.EnrolledSubjects.Add(new EnrolledSubject { SubjectID = sid, SubjectName = name });
                    Console.WriteLine("→ Subject added.");
                }

                Console.Write("Add another? (y/n): ");
                more = Console.ReadLine()?.Trim().ToLower() == "y";
            }
        }

        private static void EnterGrades()
        {
            var idx = SelectStudent();
            if (!idx.HasValue) return;
            var student = students[idx.Value];

            if (student.EnrolledSubjects.Count == 0)
            {
                Console.WriteLine("No subjects enrolled yet.");
                return;
            }

            Console.Clear();
            Console.WriteLine($"Entering grades for: {student.LastName}, {student.FirstName}\n");

            for (int i = 0; i < student.EnrolledSubjects.Count; i++)
            {
                var sub = student.EnrolledSubjects[i];
                string curr = sub.Grade.HasValue ? sub.Grade.Value.ToString("F1") : "Not entered";

                Console.WriteLine($"{i + 1}. {sub.SubjectID,-10} {sub.SubjectName,-28} [{curr}]");

                while (true)
                {
                    Console.Write("   New grade (0-100) or Enter to skip: ");
                    string? input = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(input)) break;

                    if (double.TryParse(input, out double g) && g >= 0 && g <= 100)
                    {
                        sub.Grade = g;
                        Console.WriteLine("   → Grade saved.");
                        break;
                    }

                    Console.WriteLine("   → Invalid (must be 0–100).");
                }
            }
        }

        private static void ShowGrades()
        {
            var idx = SelectStudent();
            if (!idx.HasValue) return;
            var s = students[idx.Value];

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║ GRADE REPORT - {s.LastName.ToUpper()}, {s.FirstName} {s.MiddleInitial,-32} ║");
            Console.WriteLine($"║ {s.Course} - Year {s.Year,-50}║");
            Console.WriteLine($"║ Age: {s.Age,-4}  Birthdate: {s.BirthDate:yyyy-MM-dd,-32}║");
            Console.WriteLine($"║ Contact: {s.ContactNumber,-44}║");
            Console.WriteLine($"║ Address: {s.Address,-44}║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ SUBJ ID    SUBJECT NAME                          GRADE      ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════╣");

            if (s.EnrolledSubjects.Count == 0)
            {
                Console.WriteLine("║                No subjects enrolled yet...                 ║");
            }
            else
            {
                foreach (var sub in s.EnrolledSubjects)
                {
                    string g = sub.Grade.HasValue ? $"{sub.Grade:F1}" : "Not entered";
                    Console.WriteLine($"║ {sub.SubjectID,-10} {sub.SubjectName,-36} {g,-10} ║");
                }
            }

            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        }
    }
}