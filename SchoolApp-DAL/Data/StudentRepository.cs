using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SchoolApp_DAL.Data.Interfaces;
using SchoolApp_DAL.Models;

namespace SchoolApp_DAL.Data;

public class StudentRepository : IStudentRepository
{
    private readonly string _connectionString;

    public StudentRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
    }

    public async Task<IReadOnlyList<Student>> GetAllAsync()
    {
        var students = new List<Student>();
        const string sql = """
                           SELECT StudentID, StudentName, StudentSurname, StudentEmail
                           FROM dbo.Student
                           ORDER BY StudentID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            students.Add(MapStudent(reader));
        }

        return students;
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        const string sql = """
                           SELECT StudentID, StudentName, StudentSurname, StudentEmail
                           FROM dbo.Student
                           WHERE StudentID = @StudentID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StudentID", id);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? MapStudent(reader) : null;
    }

    public async Task<Student?> GetByNameAsync(string studentName)
    {
        const string sql = """
                           SELECT TOP(1) StudentID, StudentName, StudentSurname, StudentEmail
                           FROM dbo.Student
                           WHERE StudentName = @StudentName
                           ORDER BY StudentID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StudentName", studentName);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? MapStudent(reader) : null;
    }

    public async Task<Student?> GetByEmailAsync(string studentEmail)
    {
        const string sql = """
                           SELECT TOP(1) StudentID, StudentName, StudentSurname, StudentEmail
                           FROM dbo.Student
                           WHERE StudentEmail = @StudentEmail
                           ORDER BY StudentID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StudentEmail", studentEmail);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? MapStudent(reader) : null;
    }

    public async Task<int> CreateAsync(Student student)
    {
        const string sql = """
                           INSERT INTO dbo.Student (StudentName, StudentSurname, StudentEmail)
                           VALUES (@StudentName, @StudentSurname, @StudentEmail);
                           SELECT CAST(SCOPE_IDENTITY() AS INT);
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StudentName", student.StudentName);
        command.Parameters.AddWithValue("@StudentSurname", student.StudentSurname);
        command.Parameters.AddWithValue("@StudentEmail", student.StudentEmail);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateAsync(Student student)
    {
        const string sql = """
                           UPDATE dbo.Student
                           SET StudentName = @StudentName,
                               StudentSurname = @StudentSurname,
                               StudentEmail = @StudentEmail
                           WHERE StudentID = @StudentID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StudentID", student.StudentID);
        command.Parameters.AddWithValue("@StudentName", student.StudentName);
        command.Parameters.AddWithValue("@StudentSurname", student.StudentSurname);
        command.Parameters.AddWithValue("@StudentEmail", student.StudentEmail);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = """
                           DELETE FROM dbo.Student
                           WHERE StudentID = @StudentID;
                           """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@StudentID", id);

        await connection.OpenAsync();
        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
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
