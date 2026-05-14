using System.ComponentModel.DataAnnotations;

namespace SchoolApp_MVC.Dtos.Courses;

public class CourseCreateDto
{
    [Required]
    [StringLength(200)]
    public string CourseName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string CourseCode { get; set; } = string.Empty;

    [Range(1, 30)]
    public int CourseCredit { get; set; }
}
