using SchoolApp_MVC.Dtos.Courses;

namespace SchoolApp_MVC.Services.Interfaces;

public interface ICourseService
{
    Task<IReadOnlyList<CourseReadDto>> GetAllAsync();
    Task<CourseReadDto?> GetByIdAsync(int id);
    Task<(bool Success, string? ErrorMessage)> CreateAsync(CourseCreateDto dto);
    Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, CourseUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
