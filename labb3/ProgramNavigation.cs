using labb3.Data;
using labb3.Models;
using System;
using System.Linq;

namespace labb3
{
    class ProgramNavigation
    {
        // Program navigation
        public void Navigate()
        {
            while (true) // Infinite loop until exit
            {
                Console.Clear();
                Console.WriteLine("Labb3");
                Console.WriteLine("1. Show students");
                Console.WriteLine("2. Show staff");
                Console.WriteLine("3. Show classes");
                Console.WriteLine("4. Show grades");
                Console.WriteLine("5. Show courses");
                Console.WriteLine("6. Add Student");
                Console.WriteLine("7. Add Staff");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ConnectToDb(GetStudents);
                        break;
                    case "2":
                        ConnectToDb(GetStaff);
                        break;
                    case "3":
                        ConnectToDb(GetClass);
                        break;
                    case "4":
                        ConnectToDb(GetGrades);
                        break;
                    case "5":
                        ConnectToDb(GetCourses);
                        break;
                    case "6":
                        ConnectToDb(AddStudent);
                        break;
                    case "7":
                        ConnectToDb(AddStaff);
                        break;
                    case "0":
                        Console.WriteLine("Exiting program. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        // Connection between DB and program
        public void ConnectToDb(Action<SchoolDBContext> action)
        {
            using (var context = new SchoolDBContext())
            {
                action(context);
            }
        }

        // Retrieve and display students
        public void GetStudents(SchoolDBContext context)
        {

            while (true)
            {
                Console.Clear();

                Console.WriteLine("Choose how to sort the students:");
                Console.WriteLine("1. Sort by first name ascending");
                Console.WriteLine("2. Sort by first name descending");
                Console.WriteLine("3. Sort by last name ascending");
                Console.WriteLine("4. Sort by last name descending");
                Console.WriteLine("Choose an option: ");

                int userChoice;
                bool isValidInput = int.TryParse(Console.ReadLine()?.Trim(), out userChoice);

                if (isValidInput)
                {
                    switch (userChoice)
                    {
                        case 1:
                            var studentsByFirstNameAsc = context.Students.OrderBy(s => s.FirstName).ToList();
                            Console.Clear();
                            Console.WriteLine("Students sorted by first name (ascending):");
                            foreach (var student in studentsByFirstNameAsc)
                            {
                                Console.WriteLine($"{student.FirstName} {student.LastName}");
                            }
                            break;
                        case 2:
                            var studentsByFirstNameDesc = context.Students.OrderByDescending(s => s.FirstName).ToList();
                            Console.Clear();
                            Console.WriteLine("Students sorted by first name (descending):");
                            foreach (var student in studentsByFirstNameDesc)
                            {
                                Console.WriteLine($"{student.FirstName} {student.LastName}");
                            }
                            break;
                        case 3:
                            var studentsByLastNameAsc = context.Students.OrderBy(s => s.LastName).ToList();
                            Console.Clear();
                            Console.WriteLine("Students sorted by last name (ascending):");
                            foreach (var student in studentsByLastNameAsc)
                            {
                                Console.WriteLine($"{student.FirstName} {student.LastName}");
                            }
                            break;
                        case 4:
                            var studentsByLastNameDesc = context.Students.OrderByDescending(s => s.LastName).ToList();
                            Console.Clear();
                            Console.WriteLine("Students sorted by last name (descending):");
                            foreach (var student in studentsByLastNameDesc)
                            {
                                Console.WriteLine($"{student.FirstName} {student.LastName}");
                            }
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please choose between 1 and 4.");
                            continue;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
                }

            }
        }

        public void GetStaff(SchoolDBContext context)
        {
            var PersonalList = context.Personals.ToList();

            var PersonalPositions = PersonalList
                .Select(p => p.Position)
                .Distinct()
                .ToList();

            IEnumerable<Personal> filteredList = new List<Personal>();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose a category to display staff:");
                Console.WriteLine("1. All");
                Console.WriteLine("2. Principal");
                Console.WriteLine("3. Teachers");
                Console.WriteLine("4. Administrators");

                int userChoice;
                bool isValidInput = int.TryParse(Console.ReadLine()?.Trim(), out userChoice);

                if (isValidInput)
                {
                    switch (userChoice)
                    {
                        case 1:
                            filteredList = PersonalList;
                            break;
                        case 2:
                            filteredList = PersonalList.Where(p => p.Position == "Principal");
                            break;
                        case 3:
                            filteredList = PersonalList.Where(p => p.Position == "Teacher");
                            break;
                        case 4:
                            filteredList = PersonalList.Where(p => p.Position == "Administrator");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please choose between 1 and 4.");
                            continue;
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
                }
            }

            Console.Clear();
            Console.WriteLine("Staff List:");

            foreach (var Personal in filteredList)
            {
                Console.WriteLine($"{Personal.FirstName} {Personal.LastName} - {Personal.Position}");
            }
        }

        public void GetClass(SchoolDBContext context)
        {
            // Retrieve all distinct classes from the database
            var classes = context.Students
                .Select(s => s.Class)
                .Distinct()
                .ToList();

            if (!classes.Any())
            {
                Console.WriteLine("No classes available.");
                return;
            }

            Console.Clear();
            Console.WriteLine("Classes:");
            for (int i = 0; i < classes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {classes[i]}");
            }

            Console.Write("\nSelect a class by number: ");
            if (int.TryParse(Console.ReadLine(), out int classChoice) && classChoice > 0 && classChoice <= classes.Count)
            {
                string selectedClass = classes[classChoice - 1];
                Console.Clear();
                Console.WriteLine($"Students in class {selectedClass}:");

                // Retrieve students in the selected class
                var studentsInClass = context.Students
                    .Where(s => s.Class == selectedClass)
                    .ToList();

                if (studentsInClass.Any())
                {
                    foreach (var student in studentsInClass)
                    {
                        Console.WriteLine($"{student.FirstName} {student.LastName}");
                    }
                }
                else
                {
                    Console.WriteLine($"No students found in class {selectedClass}.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Returning to the main menu.");
            }
        }

        public void GetGrades(SchoolDBContext context)
        {
            // Retrieve all grades from last month
            var grades = context.Grades
                .Where(g => g.GradeDate >= DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)))
                .ToList();

            // Retrieve all Students
            var students = context.Students.ToList();

            // Retrieve all Staff that are teachers
            var staff = context.Personals.Where(p => p.Position == "Teacher").ToList();

            Console.Clear();
            Console.WriteLine("Grades:");

            Console.Clear();
            Console.WriteLine("Grades set in the last month:");
            foreach (var entry in grades)
            {
                var student = students.FirstOrDefault(s => s.StudentId == entry.StudentId);
                var teacher = staff.FirstOrDefault(s => s.PersonalId == entry.TeacherId);
                var course = context.Courses.FirstOrDefault(c => c.CourseId == entry.CourseId);

                Console.WriteLine($"{student?.FirstName} {student?.LastName} got {entry.Grade1} in {course?.CourseName} by {teacher?.FirstName} {teacher?.LastName} on {entry?.GradeDate}");
            }
        }

        public void GetCourses(SchoolDBContext context)
        {
            Console.Clear();
            var grades = context.Grades.ToList();
            var courses = context.Courses.ToList();

            // Grades mapping to numerical values and finer grade intervals
            Dictionary<string, double> gradeToValue = new Dictionary<string, double>
            {
                { "A", 4.0 }, { "B", 3.0 }, { "C", 2.0 }, { "D", 1.0 }, { "F", 0.0 }
            };

            Dictionary<double, string> valueToGrade = new Dictionary<double, string>
            {
                { 4.0, "A" }, { 3.75, "A-" },
                { 3.5, "B+" }, { 3.0, "B" }, { 2.75, "B-" },
                { 2.5, "C+" }, { 2.0, "C" }, { 1.75, "C-" },
                { 1.5, "D+" }, { 1.0, "D" }, { 0.0, "F" }
            };

            string GetGradeFromValue(double value)
            {
                foreach (var grade in valueToGrade.OrderByDescending(g => g.Key))
                {
                    if (value >= grade.Key)
                        return grade.Value;
                }
                return "F"; // Default to F if no match
            }

            // Dictionary to store course name and grade summary
            var courseGrades = new Dictionary<string, (string Average, string Highest, string Lowest)>();

            foreach (var course in courses)
            {
                var courseGradesList = grades.Where(g => g.CourseId == course.CourseId).ToList();

                if (courseGradesList.Any())
                {
                    var numericalGrades = courseGradesList.Select(g => gradeToValue[g.Grade1]).ToList();
                    var averageGradeValue = numericalGrades.Average();
                    var highestGradeValue = numericalGrades.Max();
                    var lowestGradeValue = numericalGrades.Min();

                    // Get letter grades from values
                    var averageGrade = GetGradeFromValue(averageGradeValue);
                    var highestGrade = GetGradeFromValue(highestGradeValue);
                    var lowestGrade = GetGradeFromValue(lowestGradeValue);

                    // Store the result
                    courseGrades.Add(course.CourseName, (averageGrade, highestGrade, lowestGrade));
                }
                else
                {
                    courseGrades.Add(course.CourseName, ("N/A", "N/A", "N/A"));
                }

            }

            // Display styled output
            Console.WriteLine("COURSES GRADES SUMMARY");
            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"{"Course",-15} {"Average Grade",-20} {"Highest Grade",-15} {"Lowest Grade",-15}");
            Console.WriteLine(new string('-', 70));

            foreach (var course in courseGrades)
            {
                var (average, highest, lowest) = course.Value;
                Console.WriteLine($"{course.Key,-15} {average,-20} {highest,-15} {lowest,-15}");
            }

            Console.WriteLine(new string('-', 70));
            Console.WriteLine("Press any key to continue...");
        }

        public void AddStudent(SchoolDBContext context)
        {
            Console.Clear();
            Console.WriteLine("Add Student");
            Console.WriteLine("Enter the student's first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter the student's last name:");
            string lastName = Console.ReadLine();
            Console.WriteLine("Enter the student's class:");
            string studentClass = Console.ReadLine();
            Console.WriteLine("Enter the student's personal number:");
            string personalNumber = Console.ReadLine();

            var newStudent = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Class = studentClass,
                PersonalNumber = personalNumber
            };

            context.Students.Add(newStudent);
            context.SaveChanges();
            Console.WriteLine("Student added successfully.");
        }

        public void AddStaff(SchoolDBContext context)
        {
            Console.Clear();
            Console.WriteLine("Add Staff");
            Console.WriteLine("Enter the staff's first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter the staff's last name:");
            string lastName = Console.ReadLine();

            // Display position options
            Console.WriteLine("Select the staff's position:");
            Console.WriteLine("1. Principal");
            Console.WriteLine("2. Teacher");
            Console.WriteLine("3. Administrator");

            string position = "";
            bool validInput = false;

            // Loop until a valid position is selected
            while (!validInput)
            {
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        position = "Principal";
                        validInput = true;
                        break;
                    case "2":
                        position = "Teacher";
                        validInput = true;
                        break;
                    case "3":
                        position = "Administrator";
                        validInput = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please select 1, 2, or 3.");
                        break;
                }
            }

            var newStaff = new Personal
            {
                FirstName = firstName,
                LastName = lastName,
                Position = position
            };

            context.Personals.Add(newStaff);
            context.SaveChanges();
            Console.WriteLine("Staff added successfully.");
        }


    }
}