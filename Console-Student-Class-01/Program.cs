using Microsoft.Extensions.Configuration;
using SchoolApp_DAL.Data;

namespace Console_Student_Class_01
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var studentRepository = new StudentRepository(configuration);
            var courseRepository = new CourseRepository(configuration);
            var studentService = new StudentService(studentRepository);
            var courseService = new CourseService(courseRepository);

            var exit = false;

            while (!exit)
            {
                try
                {
                    var totalStudentCount = await studentService.GetStudentCountAsync();
                    var totalCourseCount = await courseService.GetCourseCountAsync();

                    Console.WriteLine("---------------------------");
                    Console.WriteLine("Student & Course System");
                    Console.WriteLine("Total Student Count = " + totalStudentCount);
                    Console.WriteLine("Total Course Count = " + totalCourseCount);
                    Console.WriteLine("1 - Register Student");
                    Console.WriteLine("2 - Display All Students");
                    Console.WriteLine("3 - Find Student by ID");
                    Console.WriteLine("4 - Find Student by Name");
                    Console.WriteLine("5 - Register Course");
                    Console.WriteLine("6 - Display All Courses");
                    Console.WriteLine("7 - Find Course by ID");
                    Console.WriteLine("8 - Find Course by Name");
                    Console.WriteLine("9 - Exit");

                    Console.Write("Please enter your choice: ");
                    var userChoice = Console.ReadLine();

                    if (userChoice == "1")
                    {
                        var student = Student.RegisterFromConsole();
                        await studentService.RegisterStudentAsync(student);
                        Console.WriteLine($"Student Registered: {student.StudentName} {student.StudentSurname} (ID: {student.StudentID})");
                    }
                    else if (userChoice == "2")
                    {
                        await studentService.DisplayStudentListAsync();
                    }
                    else if (userChoice == "3")
                    {
                        Console.Write("Enter Student ID to find: ");
                        var studentID = Convert.ToInt32(Console.ReadLine());
                        await studentService.FindStudentByIdAsync(studentID);
                    }
                    else if (userChoice == "4")
                    {
                        Console.Write("Enter Student Name to find: ");
                        var studentName = Console.ReadLine() ?? string.Empty;
                        await studentService.FindStudentByNameAsync(studentName);
                    }
                    else if (userChoice == "5")
                    {
                        var course = Course.RegisterFromConsole();
                        await courseService.RegisterCourseAsync(course);
                        Console.WriteLine($"Course Registered: {course.CourseName} ({course.CourseCode})");
                    }
                    else if (userChoice == "6")
                    {
                        await courseService.DisplayCourseListAsync();
                    }
                    else if (userChoice == "7")
                    {
                        Console.Write("Enter Course ID to find: ");
                        var courseID = Convert.ToInt32(Console.ReadLine());
                        await courseService.FindCourseByIdAsync(courseID);
                    }
                    else if (userChoice == "8")
                    {
                        Console.Write("Enter Course Name to find: ");
                        var courseName = Console.ReadLine() ?? string.Empty;
                        await courseService.FindCourseByNameAsync(courseName);
                    }
                    else if (userChoice == "9" || string.Equals(userChoice, "exit", StringComparison.OrdinalIgnoreCase))
                    {
                        exit = true;
                        Console.WriteLine("Exiting the program...");
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice, please try again.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
