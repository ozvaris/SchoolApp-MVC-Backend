using SchoolApp_MVC.Dtos.Students;

namespace SchoolApp_MVC.ApiClients.Interfaces;

public interface IStudentApiClient
{
    Task<IReadOnlyList<StudentReadDto>> GetAllAsync();
    Task<StudentReadDto?> GetByIdAsync(int id);
    Task<(bool Success, string? ErrorMessage)> CreateAsync(StudentCreateDto dto);
    Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, StudentUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
