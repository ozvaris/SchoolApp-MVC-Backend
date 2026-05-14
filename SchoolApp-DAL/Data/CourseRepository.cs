using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SchoolApp_DAL.Data.Interfaces;
using SchoolApp_DAL.Models;

namespace SchoolApp_DAL.Data;

public class CourseRepository : ICourseRepository
{
    private readonly string _connectionString;

    public CourseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
    }

    public async Task<IReadOnlyList<Course>> GetAllAsync()
    {
        var courses = new List<Course>();
        const string sql = """
                           SELECT CourseID, CourseName, CourseCode, CourseCredit
                           FROM dbo.Course
                           ORDER BY CourseID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courses.Add(MapCourse(reader));
        }

        return courses;
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        const string sql = """
                           SELECT CourseID, CourseName, CourseCode, CourseCredit
                           FROM dbo.Course
                           WHERE CourseID = @CourseID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CourseID", id);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? MapCourse(reader) : null;
    }

    public async Task<Course?> GetByNameAsync(string courseName)
    {
        const string sql = """
                           SELECT TOP(1) CourseID, CourseName, CourseCode, CourseCredit
                           FROM dbo.Course
                           WHERE CourseName = @CourseName
                           ORDER BY CourseID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CourseName", courseName);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? MapCourse(reader) : null;
    }

    public async Task<Course?> GetByCodeAsync(string courseCode)
    {
        const string sql = """
                           SELECT CourseID, CourseName, CourseCode, CourseCredit
                           FROM dbo.Course
                           WHERE CourseCode = @CourseCode;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CourseCode", courseCode);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? MapCourse(reader) : null;
    }

    public async Task<int> CreateAsync(Course course)
    {
        const string sql = """
                           INSERT INTO dbo.Course (CourseName, CourseCode, CourseCredit)
                           VALUES (@CourseName, @CourseCode, @CourseCredit);
                           SELECT CAST(SCOPE_IDENTITY() AS INT);
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CourseName", course.CourseName);
        command.Parameters.AddWithValue("@CourseCode", course.CourseCode);
        command.Parameters.AddWithValue("@CourseCredit", course.CourseCredit);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateAsync(Course course)
    {
        const string sql = """
                           UPDATE dbo.Course
                           SET CourseName = @CourseName,
                               CourseCode = @CourseCode,
                               CourseCredit = @CourseCredit
                           WHERE CourseID = @CourseID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CourseID", course.CourseID);
        command.Parameters.AddWithValue("@CourseName", course.CourseName);
        command.Parameters.AddWithValue("@CourseCode", course.CourseCode);
        command.Parameters.AddWithValue("@CourseCredit", course.CourseCredit);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """
                           DELETE FROM dbo.Course
                           WHERE CourseID = @CourseID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CourseID", id);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    private static Course MapCourse(SqlDataReader reader)
    {
        return new Course
        {
            CourseID = reader.GetInt32(reader.GetOrdinal("CourseID")),
            CourseName = reader.GetString(reader.GetOrdinal("CourseName")),
            CourseCode = reader.GetString(reader.GetOrdinal("CourseCode")),
            CourseCredit = reader.GetInt32(reader.GetOrdinal("CourseCredit"))
        };
    }
}
