namespace Console_Student_Class_01
{
    public class StudentService
    {
        private readonly SchoolApp_DAL.Data.Interfaces.IStudentRepository _studentRepository;

        public StudentService(SchoolApp_DAL.Data.Interfaces.IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<int> GetStudentCountAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Count;
        }

        public async Task RegisterStudentAsync(Student student)
        {
            var dbStudent = new SchoolApp_DAL.Models.Student
            {
                StudentName = student.StudentName,
                StudentSurname = student.StudentSurname,
                StudentEmail = student.StudentEmail
            };

            student.StudentID = await _studentRepository.CreateAsync(dbStudent);
        }

        public async Task DisplayStudentListAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            if (students.Count == 0)
            {
                Console.WriteLine("\nNo students registered.");
                return;
            }

            Console.WriteLine("\nRegistered Students:");
            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.StudentID}, Name: {student.StudentName}");
            }
        }

        public async Task FindStudentByIdAsync(int studentID)
        {
            var student = await _studentRepository.GetByIdAsync(studentID);
            if (student is null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.WriteLine($"Student ID: {student.StudentID}");
            Console.WriteLine($"Student Name: {student.StudentName}");
            Console.WriteLine($"Student Surname: {student.StudentSurname}");
            Console.WriteLine($"Student Email: {student.StudentEmail}");
        }

        public async Task FindStudentByNameAsync(string studentName)
        {
            var normalizedName = studentName?.Trim();
            if (string.IsNullOrWhiteSpace(normalizedName))
            {
                Console.WriteLine("Student name is required.");
                return;
            }

            var student = await _studentRepository.GetByNameAsync(normalizedName);

            if (student is null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.WriteLine($"Student ID: {student.StudentID}");
            Console.WriteLine($"Student Name: {student.StudentName}");
            Console.WriteLine($"Student Surname: {student.StudentSurname}");
            Console.WriteLine($"Student Email: {student.StudentEmail}");
        }
    }
}
