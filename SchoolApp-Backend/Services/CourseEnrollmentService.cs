using SchoolApp_Backend.Dtos.CourseEnrollments;
using SchoolApp_Backend.Dtos.Students;
using SchoolApp_Backend.Services.Interfaces;
using SchoolApp_DAL.Data.Interfaces;
using SchoolApp_DAL.Models;

namespace SchoolApp_Backend.Services;

public class CourseEnrollmentService : ICourseEnrollmentService
{
    private readonly ICourseEnrollmentRepository _repository;
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentRepository _studentRepository;

    public CourseEnrollmentService(
        ICourseEnrollmentRepository repository,
        ICourseRepository courseRepository,
        IStudentRepository studentRepository)
    {
        _repository = repository;
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
    }

    public async Task<IReadOnlyList<CourseEnrollmentReadDto>> GetAllAsync()
    {
        var enrollments = await _repository.GetAllAsync();
        return enrollments.Select(ToReadDto).ToList();
    }

    public async Task<IReadOnlyList<CourseEnrollmentReadDto>> GetByCourseIdAsync(int courseId)
    {
        var enrollments = await _repository.GetByCourseIdAsync(courseId);
        return enrollments.Select(ToReadDto).ToList();
    }

    public async Task<IReadOnlyList<StudentReadDto>> GetAvailableStudentsByCourseIdAsync(int courseId)
    {
        var students = await _repository.GetAvailableStudentsByCourseIdAsync(courseId);
        return students.Select(ToStudentReadDto).ToList();
    }

    public async Task<CourseEnrollmentReadDto?> GetByIdAsync(int id)
    {
        var enrollment = await _repository.GetByIdAsync(id);
        return enrollment is null ? null : ToReadDto(enrollment);
    }

    public async Task<(bool Success, string? ErrorMessage)> CreateAsync(CourseEnrollmentCreateDto dto)
    {
        var course = await _courseRepository.GetByIdAsync(dto.CourseID);
        if (course is null)
        {
            return (false, "Course not found.");
        }

        var student = await _studentRepository.GetByIdAsync(dto.StudentID);
        if (student is null)
        {
            return (false, "Student not found.");
        }

        var existing = await _repository.GetByCourseAndStudentAsync(dto.CourseID, dto.StudentID);
        if (existing is not null)
        {
            return (false, "Student is already enrolled in this course.");
        }

        var enrollment = new CourseEnrollment
        {
            CourseID = dto.CourseID,
            StudentID = dto.StudentID,
            EnrollmentDate = dto.EnrollmentDate
        };

        await _repository.CreateAsync(enrollment);
        return (true, null);
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, CourseEnrollmentUpdateDto dto)
    {
        var existingEnrollment = await _repository.GetByIdAsync(id);
        if (existingEnrollment is null)
        {
            return (false, "Course enrollment not found.");
        }

        existingEnrollment.EnrollmentDate = dto.EnrollmentDate;

        var updated = await _repository.UpdateAsync(existingEnrollment);
        return updated ? (true, null) : (false, "Course enrollment update failed.");
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    private static CourseEnrollmentReadDto ToReadDto(CourseEnrollment enrollment)
    {
        return new CourseEnrollmentReadDto
        {
            CourseEnrollmentID = enrollment.CourseEnrollmentID,
            StudentID = enrollment.StudentID,
            CourseID = enrollment.CourseID,
            EnrollmentDate = enrollment.EnrollmentDate,
            StudentName = enrollment.StudentName,
            StudentSurname = enrollment.StudentSurname,
            StudentEmail = enrollment.StudentEmail,
            CourseName = enrollment.CourseName,
            CourseCode = enrollment.CourseCode
        };
    }

    private static StudentReadDto ToStudentReadDto(Student student)
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
