namespace SchoolApp_MVC.Dtos.Courses;

public class CourseReadDto
{
    public int CourseID { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseCode { get; set; } = string.Empty;
    public int CourseCredit { get; set; }
}
