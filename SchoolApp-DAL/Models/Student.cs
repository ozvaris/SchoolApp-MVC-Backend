namespace SchoolApp_DAL.Models;

public class Student
{
    public int StudentID { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentSurname { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
}
