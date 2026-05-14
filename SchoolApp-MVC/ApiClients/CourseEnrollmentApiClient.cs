using System.Net;
using System.Net.Http.Json;
using SchoolApp_MVC.ApiClients.Interfaces;
using SchoolApp_MVC.Dtos.CourseEnrollments;
using SchoolApp_MVC.Dtos.Students;

namespace SchoolApp_MVC.ApiClients;

public class CourseEnrollmentApiClient : ICourseEnrollmentApiClient
{
    private readonly HttpClient _httpClient;

    public CourseEnrollmentApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<CourseEnrollmentReadDto>> GetAllAsync()
    {
        var enrollments = await _httpClient.GetFromJsonAsync<IReadOnlyList<CourseEnrollmentReadDto>>("api/courseenrollments");
        return enrollments ?? [];
    }

    public async Task<IReadOnlyList<CourseEnrollmentReadDto>> GetByCourseIdAsync(int courseId)
    {
        var enrollments = await _httpClient.GetFromJsonAsync<IReadOnlyList<CourseEnrollmentReadDto>>($"api/courseenrollments?courseId={courseId}");
        return enrollments ?? [];
    }

    public async Task<IReadOnlyList<StudentReadDto>> GetAvailableStudentsByCourseIdAsync(int courseId)
    {
        var students = await _httpClient.GetFromJsonAsync<IReadOnlyList<StudentReadDto>>($"api/courseenrollments/available-students/{courseId}");
        return students ?? [];
    }

    public async Task<CourseEnrollmentReadDto?> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/courseenrollments/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CourseEnrollmentReadDto>();
    }

    public async Task<(bool Success, string? ErrorMessage)> CreateAsync(CourseEnrollmentCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/courseenrollments", dto);
        return await ToResultAsync(response);
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, CourseEnrollmentUpdateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/courseenrollments/{id}", dto);
        return await ToResultAsync(response);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/courseenrollments/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }

    private static async Task<(bool Success, string? ErrorMessage)> ToResultAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return (false, "Course enrollment not found.");
        }

        var error = await response.Content.ReadFromJsonAsync<ApiError>();
        return (false, error?.ErrorMessage ?? "Course enrollment operation failed.");
    }

    private sealed class ApiError
    {
        public string? ErrorMessage { get; set; }
    }
}
