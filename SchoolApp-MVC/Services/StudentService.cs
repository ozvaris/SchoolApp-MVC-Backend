using SchoolApp_DAL.Data.Interfaces;
using SchoolApp_DAL.Models;
using SchoolApp_MVC.Dtos.Students;
using SchoolApp_MVC.Services.Interfaces;

namespace SchoolApp_MVC.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<StudentReadDto>> GetAllAsync()
    {
        var students = await _repository.GetAllAsync();
        return students.Select(ToReadDto).ToList();
    }

    public async Task<StudentReadDto?> GetByIdAsync(int id)
    {
        var student = await _repository.GetByIdAsync(id);
        return student is null ? null : ToReadDto(student);
    }

    public async Task<(bool Success, string? ErrorMessage)> CreateAsync(StudentCreateDto dto)
    {
        var normalizedEmail = dto.StudentEmail.Trim();
        var existing = await _repository.GetByEmailAsync(normalizedEmail);
        if (existing is not null)
        {
            return (false, "Student email already exists.");
        }

        var student = new Student
        {
            StudentName = dto.StudentName.Trim(),
            StudentSurname = dto.StudentSurname.Trim(),
            StudentEmail = normalizedEmail
        };

        await _repository.CreateAsync(student);
        return (true, null);
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, StudentUpdateDto dto)
    {
        var existingStudent = await _repository.GetByIdAsync(id);
        if (existingStudent is null)
        {
            return (false, "Student not found.");
        }

        var normalizedEmail = dto.StudentEmail.Trim();
        var duplicateEmailStudent = await _repository.GetByEmailAsync(normalizedEmail);
        if (duplicateEmailStudent is not null && duplicateEmailStudent.StudentID != id)
        {
            return (false, "Student email already exists.");
        }

        existingStudent.StudentName = dto.StudentName.Trim();
        existingStudent.StudentSurname = dto.StudentSurname.Trim();
        existingStudent.StudentEmail = normalizedEmail;

        var updated = await _repository.UpdateAsync(existingStudent);
        return updated ? (true, null) : (false, "Student update failed.");
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    private static StudentReadDto ToReadDto(Student student)
    {
        return new StudentReadDto
        {
            StudentID = student.StudentID,
            StudentName = student.StudentName,
            StudentSurname = student.StudentSurname,
            StudentEmail = student.StudentEmail
        };
    }
}
