using System.Net;
using System.Net.Http.Json;
using SchoolApp_MVC.ApiClients.Interfaces;
using SchoolApp_MVC.Dtos.Courses;

namespace SchoolApp_MVC.ApiClients;

public class CourseApiClient : ICourseApiClient
{
    private readonly HttpClient _httpClient;

    public CourseApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<CourseReadDto>> GetAllAsync()
    {
        var courses = await _httpClient.GetFromJsonAsync<IReadOnlyList<CourseReadDto>>("api/courses");
        return courses ?? [];
    }

    public async Task<CourseReadDto?> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/courses/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CourseReadDto>();
    }

    public async Task<(bool Success, string? ErrorMessage)> CreateAsync(CourseCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/courses", dto);
        return await ToResultAsync(response);
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, CourseUpdateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/courses/{id}", dto);
        return await ToResultAsync(response);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/courses/{id}");
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
            return (false, "Course not found.");
        }

        var error = await response.Content.ReadFromJsonAsync<ApiError>();
        return (false, error?.ErrorMessage ?? "Course operation failed.");
    }

    private sealed class ApiError
    {
        public string? ErrorMessage { get; set; }
    }
}
