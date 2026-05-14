namespace Console_Student_Class_01
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public int CourseCredit { get; set; }

        public static Course RegisterFromConsole()
        {
            Console.WriteLine("Enter Course Name:");
            var courseName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(courseName))
            {
                throw new ArgumentException("Course Name is required.");
            }

            Console.WriteLine("Enter Course Code:");
            var courseCode = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(courseCode))
            {
                throw new ArgumentException("Course Code is required.");
            }

            Console.WriteLine("Enter Course Credit:");
            if (!int.TryParse(Console.ReadLine(), out var courseCredit))
            {
                throw new ArgumentException("Course Credit must be a number.");
            }

            if (courseCredit <= 0)
            {
                throw new ArgumentException("Course Credit must be greater than zero.");
            }

            return new Course
            {
                CourseName = courseName.Trim(),
                CourseCode = courseCode.Trim().ToUpperInvariant(),
                CourseCredit = courseCredit
            };
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Course ID: {CourseID}");
            Console.WriteLine($"Course Name: {CourseName}");
            Console.WriteLine($"Course Code: {CourseCode}");
            Console.WriteLine($"Course Credit: {CourseCredit}");
        }
    }
}
