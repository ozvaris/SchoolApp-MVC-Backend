using SchoolApp_Backend.Dtos.Students;

namespace SchoolApp_Backend.Services.Interfaces;

public interface IStudentService
{
    Task<IReadOnlyList<StudentReadDto>> GetAllAsync();
    Task<StudentReadDto?> GetByIdAsync(int id);
    Task<(bool Success, string? ErrorMessage)> CreateAsync(StudentCreateDto dto);
    Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, StudentUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
