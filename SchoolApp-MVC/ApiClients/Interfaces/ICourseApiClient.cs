using SchoolApp_MVC.Dtos.Courses;

namespace SchoolApp_MVC.ApiClients.Interfaces;

public interface ICourseApiClient
{
    Task<IReadOnlyList<CourseReadDto>> GetAllAsync();
    Task<CourseReadDto?> GetByIdAsync(int id);
    Task<(bool Success, string? ErrorMessage)> CreateAsync(CourseCreateDto dto);
    Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, CourseUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
