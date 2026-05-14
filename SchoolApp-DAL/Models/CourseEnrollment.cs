namespace SchoolApp_DAL.Models;

public class CourseEnrollment
{
    public int CourseEnrollmentID { get; set; }
    public int StudentID { get; set; }
    public int CourseID { get; set; }
    public DateTime? EnrollmentDate { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentSurname { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
}
