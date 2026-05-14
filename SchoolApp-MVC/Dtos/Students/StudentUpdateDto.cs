using System.ComponentModel.DataAnnotations;

namespace SchoolApp_MVC.Dtos.Students;

public class StudentUpdateDto
{
    [Required]
    public int StudentID { get; set; }

    [Required]
    [StringLength(100)]
    public string StudentName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string StudentSurname { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    [EmailAddress]
    public string StudentEmail { get; set; } = string.Empty;
}
