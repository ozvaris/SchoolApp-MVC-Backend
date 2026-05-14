using System.ComponentModel.DataAnnotations;

namespace SchoolApp_MVC.Dtos.CourseEnrollments;

public class CourseEnrollmentCreateDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Please select a student.")]
    public int StudentID { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please select a course.")]
    public int CourseID { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EnrollmentDate { get; set; }
}
