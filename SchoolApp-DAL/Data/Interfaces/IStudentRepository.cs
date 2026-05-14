using SchoolApp_DAL.Models;

namespace SchoolApp_DAL.Data.Interfaces;

public interface IStudentRepository
{
    Task<IReadOnlyList<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task<Student?> GetByNameAsync(string studentName);
    Task<Student?> GetByEmailAsync(string studentEmail);
    Task<int> CreateAsync(Student student);
    Task<bool> UpdateAsync(Student student);
    Task<bool> DeleteAsync(int id);
}
