using System.ComponentModel.DataAnnotations;

namespace SchoolApp_MVC.Dtos.Courses;

public class CourseUpdateDto
{
    [Required]
    public int CourseID { get; set; }

    [Required]
    [StringLength(200)]
    public string CourseName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string CourseCode { get; set; } = string.Empty;

    [Range(1, 30)]
    public int CourseCredit { get; set; }
}
