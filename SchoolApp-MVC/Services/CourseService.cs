using SchoolApp_DAL.Data.Interfaces;
using SchoolApp_DAL.Models;
using SchoolApp_MVC.Dtos.Courses;
using SchoolApp_MVC.Services.Interfaces;

namespace SchoolApp_MVC.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _repository;

    public CourseService(ICourseRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<CourseReadDto>> GetAllAsync()
    {
        var courses = await _repository.GetAllAsync();
        return courses.Select(ToReadDto).ToList();
    }

    public async Task<CourseReadDto?> GetByIdAsync(int id)
    {
        var course = await _repository.GetByIdAsync(id);
        return course is null ? null : ToReadDto(course);
    }

    public async Task<(bool Success, string? ErrorMessage)> CreateAsync(CourseCreateDto dto)
    {
        var existing = await _repository.GetByCodeAsync(dto.CourseCode.Trim());
        if (existing is not null)
        {
            return (false, "Course code already exists.");
        }

        var course = new Course
        {
            CourseName = dto.CourseName.Trim(),
            CourseCode = dto.CourseCode.Trim().ToUpperInvariant(),
            CourseCredit = dto.CourseCredit
        };

        await _repository.CreateAsync(course);
        return (true, null);
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, CourseUpdateDto dto)
    {
        var existingCourse = await _repository.GetByIdAsync(id);
        if (existingCourse is null)
        {
            return (false, "Course not found.");
        }

        var duplicateCodeCourse = await _repository.GetByCodeAsync(dto.CourseCode.Trim());
        if (duplicateCodeCourse is not null && duplicateCodeCourse.CourseID != id)
        {
            return (false, "Course code already exists.");
        }

        existingCourse.CourseName = dto.CourseName.Trim();
        existingCourse.CourseCode = dto.CourseCode.Trim().ToUpperInvariant();
        existingCourse.CourseCredit = dto.CourseCredit;

        var updated = await _repository.UpdateAsync(existingCourse);
        return updated ? (true, null) : (false, "Course update failed.");
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    private static CourseReadDto ToReadDto(Course course)
    {
        return new CourseReadDto
        {
            CourseID = course.CourseID,
            CourseName = course.CourseName,
            CourseCode = course.CourseCode,
            CourseCredit = course.CourseCredit
        };
    }
}
