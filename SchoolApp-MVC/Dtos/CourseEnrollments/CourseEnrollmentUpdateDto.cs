using System.ComponentModel.DataAnnotations;

namespace SchoolApp_MVC.Dtos.CourseEnrollments;

public class CourseEnrollmentUpdateDto
{
    [Range(1, int.MaxValue)]
    public int CourseEnrollmentID { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EnrollmentDate { get; set; }
}
