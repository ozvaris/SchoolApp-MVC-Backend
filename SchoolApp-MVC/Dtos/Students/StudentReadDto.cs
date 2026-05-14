namespace SchoolApp_MVC.Dtos.Students;

public class StudentReadDto
{
    public int StudentID { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentSurname { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
}
