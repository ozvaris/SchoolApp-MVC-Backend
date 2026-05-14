using SchoolApp_MVC.Dtos.CourseEnrollments;
using SchoolApp_MVC.Dtos.Students;

namespace SchoolApp_MVC.ApiClients.Interfaces;

public interface ICourseEnrollmentApiClient
{
    Task<IReadOnlyList<CourseEnrollmentReadDto>> GetAllAsync();
    Task<IReadOnlyList<CourseEnrollmentReadDto>> GetByCourseIdAsync(int courseId);
    Task<IReadOnlyList<StudentReadDto>> GetAvailableStudentsByCourseIdAsync(int courseId);
    Task<CourseEnrollmentReadDto?> GetByIdAsync(int id);
    Task<(bool Success, string? ErrorMessage)> CreateAsync(CourseEnrollmentCreateDto dto);
    Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, CourseEnrollmentUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
