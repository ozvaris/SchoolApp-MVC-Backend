using SchoolApp_DAL.Models;

namespace SchoolApp_DAL.Data.Interfaces;

public interface ICourseEnrollmentRepository
{
    Task<IReadOnlyList<CourseEnrollment>> GetAllAsync();
    Task<IReadOnlyList<CourseEnrollment>> GetByCourseIdAsync(int courseId);
    Task<IReadOnlyList<Student>> GetAvailableStudentsByCourseIdAsync(int courseId);
    Task<CourseEnrollment?> GetByIdAsync(int id);
    Task<CourseEnrollment?> GetByCourseAndStudentAsync(int courseId, int studentId);
    Task<int> CreateAsync(CourseEnrollment courseEnrollment);
    Task<bool> UpdateAsync(CourseEnrollment courseEnrollment);
    Task<bool> DeleteAsync(int id);
}
