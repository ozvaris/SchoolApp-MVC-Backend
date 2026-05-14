namespace SchoolApp_DAL.Models;

public class Course
{
    public int CourseID { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public int CourseCredit { get; set; }
}
