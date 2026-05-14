namespace Console_Student_Class_01
{
    public class CourseService
    {
        private readonly SchoolApp_DAL.Data.Interfaces.ICourseRepository _courseRepository;

        public CourseService(SchoolApp_DAL.Data.Interfaces.ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<int> GetCourseCountAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.Count;
        }

        public async Task RegisterCourseAsync(Course course)
        {
            var existingCourseCode = await _courseRepository.GetByCodeAsync(course.CourseCode);
            if (existingCourseCode is not null)
            {
                throw new ArgumentException("A course with this code already exists. Course Code must be unique. Please Try Again.");
            }

            var dbCourse = new SchoolApp_DAL.Models.Course
            {
                CourseName = course.CourseName,
                CourseCode = course.CourseCode,
                CourseCredit = course.CourseCredit
            };

            await _courseRepository.CreateAsync(dbCourse);
        }

        public async Task DisplayCourseListAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            if (courses.Count == 0)
            {
                Console.WriteLine("\nNo courses registered.");
                return;
            }

            Console.WriteLine("\nRegistered Courses:");
            foreach (var course in courses)
            {
                Console.WriteLine($"ID: {course.CourseID}, Name: {course.CourseName}, Code: {course.CourseCode}, Credit: {course.CourseCredit}");
            }
        }

        public async Task FindCourseByIdAsync(int courseID)
        {
            var course = await _courseRepository.GetByIdAsync(courseID);
            if (course is null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            Console.WriteLine($"Course ID: {course.CourseID}");
            Console.WriteLine($"Course Name: {course.CourseName}");
            Console.WriteLine($"Course Code: {course.CourseCode}");
            Console.WriteLine($"Course Credit: {course.CourseCredit}");
        }

        public async Task FindCourseByNameAsync(string courseName)
        {
            var normalizedName = courseName?.Trim();
            if (string.IsNullOrWhiteSpace(normalizedName))
            {
                Console.WriteLine("Course name is required.");
                return;
            }

            var course = await _courseRepository.GetByNameAsync(normalizedName);

            if (course is null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            Console.WriteLine($"Course ID: {course.CourseID}");
            Console.WriteLine($"Course Name: {course.CourseName}");
            Console.WriteLine($"Course Code: {course.CourseCode}");
            Console.WriteLine($"Course Credit: {course.CourseCredit}");
        }
    }
}
