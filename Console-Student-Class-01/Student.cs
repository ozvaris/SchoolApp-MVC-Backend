namespace Console_Student_Class_01
{
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentSurname { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;

        public static Student RegisterFromConsole()
        {
            Console.WriteLine("\nEnter Student Name:");
            var studentName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(studentName))
            {
                throw new ArgumentException("Student Name is required.");
            }

            if (studentName.Length > 10)
            {
                throw new ArgumentException("Student Name must not exceed 10 characters.");
            }

            Console.WriteLine("Enter Student Surname (optional):");
            var StudentSurname = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Enter Student Email (optional):");
            var studentEmail = Console.ReadLine() ?? string.Empty;

            return new Student
            {
                StudentName = studentName.Trim(),
                StudentSurname = StudentSurname.Trim(),
                StudentEmail = studentEmail.Trim()
            };
        }

        public void DisplayInfo()
        {
            Console.WriteLine($"Student ID: {StudentID}");
            Console.WriteLine($"Student Name: {StudentName}");
            Console.WriteLine($"Student Surname: {StudentSurname}");
            Console.WriteLine($"Student Email: {StudentEmail}");
        }
    }
}
