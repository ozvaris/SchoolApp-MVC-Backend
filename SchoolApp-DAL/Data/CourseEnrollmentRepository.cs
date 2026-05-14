using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SchoolApp_DAL.Data.Interfaces;
using SchoolApp_DAL.Models;

namespace SchoolApp_DAL.Data;

public class CourseEnrollmentRepository : ICourseEnrollmentRepository
{
    private readonly string _connectionString;

    public CourseEnrollmentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
    }

    public async Task<IReadOnlyList<CourseEnrollment>> GetAllAsync()
    {
        var enrollments = new List<CourseEnrollment>();
        const string sql = """
                           SELECT ce.CourseEnrollmentID, ce.StudentID, ce.CourseID, ce.EnrollmentDate,
                                  s.StudentName, s.StudentSurname, s.StudentEmail,
                                  c.CourseName, c.CourseCode
                           FROM dbo.CourseEnrollment ce
                           INNER JOIN dbo.Student s ON ce.StudentID = s.StudentID
                           INNER JOIN dbo.Course c ON ce.CourseID = c.CourseID
                           ORDER BY c.CourseName, s.StudentSurname, s.StudentName;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(MapCourseEnrollment(reader));
        }

        return enrollments;
    }

    public async Task<IReadOnlyList<CourseEnrollment>> GetByCourseIdAsync(int courseId)
    {
        var enrollments = new List<CourseEnrollment>();
        const string sql = """
                           SELECT ce.CourseEnrollmentID, ce.StudentID, ce.CourseID, ce.EnrollmentDate,
                                  s.StudentName, s.StudentSurname, s.StudentEmail,
                                  c.CourseName, c.CourseCode
                           FROM dbo.CourseEnrollment ce
                           INNER JOIN dbo.Student s ON ce.StudentID = s.StudentID
                           INNER JOIN dbo.Course c ON ce.CourseID = c.CourseID
                           WHERE ce.CourseID = @CourseID
                           ORDER BY s.StudentSurname, s.StudentName, s.StudentID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CourseID", courseId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            enrollments.Add(MapCourseEnrollment(reader));
        }

        return enrollments;
    }

    public async Task<IReadOnlyList<Student>> GetAvailableStudentsByCourseIdAsync(int courseId)
    {
        var students = new List<Student>();
        const string sql = """
                           SELECT s.StudentID, s.StudentName, s.StudentSurname, s.StudentEmail
                           FROM dbo.Student s
                           WHERE NOT EXISTS (
                               SELECT 1
                               FROM dbo.CourseEnrollment ce
                               WHERE ce.StudentID = s.StudentID
                                 AND ce.CourseID = @CourseID
                           )
                           ORDER BY s.StudentSurname, s.StudentName, s.StudentID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CourseID", courseId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            students.Add(MapStudent(reader));
        }

        return students;
    }

    public async Task<CourseEnrollment?> GetByIdAsync(int id)
    {
        const string sql = """
                           SELECT ce.CourseEnrollmentID, ce.StudentID, ce.CourseID, ce.EnrollmentDate,
                                  s.StudentName, s.StudentSurname, s.StudentEmail,
                                  c.CourseName, c.CourseCode
                           FROM dbo.CourseEnrollment ce
                           INNER JOIN dbo.Student s ON ce.StudentID = s.StudentID
                           INNER JOIN dbo.Course c ON ce.CourseID = c.CourseID
                           WHERE ce.CourseEnrollmentID = @CourseEnrollmentID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CourseEnrollmentID", id);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? MapCourseEnrollment(reader) : null;
    }

    public async Task<CourseEnrollment?> GetByCourseAndStudentAsync(int courseId, int studentId)
    {
        const string sql = """
                           SELECT ce.CourseEnrollmentID, ce.StudentID, ce.CourseID, ce.EnrollmentDate,
                                  s.StudentName, s.StudentSurname, s.StudentEmail,
                                  c.CourseName, c.CourseCode
                           FROM dbo.CourseEnrollment ce
                           INNER JOIN dbo.Student s ON ce.StudentID = s.StudentID
                           INNER JOIN dbo.Course c ON ce.CourseID = c.CourseID
                           WHERE ce.CourseID = @CourseID
                             AND ce.StudentID = @StudentID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CourseID", courseId);
        command.Parameters.AddWithValue("@StudentID", studentId);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? MapCourseEnrollment(reader) : null;
    }

    public async Task<int> CreateAsync(CourseEnrollment courseEnrollment)
    {
        const string sql = """
                           INSERT INTO dbo.CourseEnrollment (StudentID, CourseID, EnrollmentDate)
                           VALUES (@StudentID, @CourseID, @EnrollmentDate);
                           SELECT CAST(SCOPE_IDENTITY() AS INT);
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StudentID", courseEnrollment.StudentID);
        command.Parameters.AddWithValue("@CourseID", courseEnrollment.CourseID);
        command.Parameters.AddWithValue("@EnrollmentDate", courseEnrollment.EnrollmentDate ?? (object)DBNull.Value);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateAsync(CourseEnrollment courseEnrollment)
    {
        const string sql = """
                           UPDATE dbo.CourseEnrollment
                           SET EnrollmentDate = @EnrollmentDate
                           WHERE CourseEnrollmentID = @CourseEnrollmentID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CourseEnrollmentID", courseEnrollment.CourseEnrollmentID);
        command.Parameters.AddWithValue("@EnrollmentDate", courseEnrollment.EnrollmentDate ?? (object)DBNull.Value);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """
                           DELETE FROM dbo.CourseEnrollment
                           WHERE CourseEnrollmentID = @CourseEnrollmentID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CourseEnrollmentID", id);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    private static CourseEnrollment MapCourseEnrollment(SqlDataReader reader)
    {
        return new CourseEnrollment
        {
            CourseEnrollmentID = reader.GetInt32(reader.GetOrdinal("CourseEnrollmentID")),
            StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
            CourseID = reader.GetInt32(reader.GetOrdinal("CourseID")),
            EnrollmentDate = reader.IsDBNull(reader.GetOrdinal("EnrollmentDate"))
                ? null
                : reader.GetDateTime(reader.GetOrdinal("EnrollmentDate")),
            StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
            StudentSurname = reader.GetString(reader.GetOrdinal("StudentSurname")),
            StudentEmail = reader.GetString(reader.GetOrdinal("StudentEmail")),
            CourseName = reader.GetString(reader.GetOrdinal("CourseName")),
            CourseCode = reader.GetString(reader.GetOrdinal("CourseCode"))
        };
    }

    private static Student MapStudent(SqlDataReader reader)
    {
        return new Student
        {
            StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
            StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
            StudentSurname = reader.GetString(reader.GetOrdinal("StudentSurname")),
            StudentEmail = reader.GetString(reader.GetOrdinal("StudentEmail"))
        };
    }
}
