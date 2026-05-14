using SchoolApp_MVC.Dtos.CourseEnrollments;
using SchoolApp_MVC.Dtos.Courses;
using SchoolApp_MVC.Dtos.Students;

namespace SchoolApp_MVC.ViewModels.CourseEnrollments;

public class CourseEnrollmentIndexViewModel
{
    public IReadOnlyList<CourseReadDto> Courses { get; set; } = [];
    public IReadOnlyList<CourseEnrollmentReadDto> Enrollments { get; set; } = [];
    public IReadOnlyList<StudentReadDto> AvailableStudents { get; set; } = [];
    public CourseEnrollmentCreateDto NewEnrollment { get; set; } = new();
    public int? SelectedCourseID { get; set; }
}
