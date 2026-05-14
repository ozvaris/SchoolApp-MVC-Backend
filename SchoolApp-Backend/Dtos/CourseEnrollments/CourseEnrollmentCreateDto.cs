using System.ComponentModel.DataAnnotations;

namespace SchoolApp_Backend.Dtos.CourseEnrollments;

public class CourseEnrollmentCreateDto
{
    [Range(1, int.MaxValue)]
    public int StudentID { get; set; }

    [Range(1, int.MaxValue)]
    public int CourseID { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EnrollmentDate { get; set; }
}
