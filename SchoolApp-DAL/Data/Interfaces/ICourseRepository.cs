using SchoolApp_DAL.Models;

namespace SchoolApp_DAL.Data.Interfaces;

public interface ICourseRepository
{
    Task<IReadOnlyList<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(int id);
    Task<Course?> GetByNameAsync(string courseName);
    Task<Course?> GetByCodeAsync(string courseCode);
    Task<int> CreateAsync(Course course);
    Task<bool> UpdateAsync(Course course);
    Task<bool> DeleteAsync(int id);
}
