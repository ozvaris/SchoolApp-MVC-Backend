using System.Net;
using System.Net.Http.Json;
using SchoolApp_MVC.ApiClients.Interfaces;
using SchoolApp_MVC.Dtos.Students;

namespace SchoolApp_MVC.ApiClients;

public class StudentApiClient : IStudentApiClient
{
    private readonly HttpClient _httpClient;

    public StudentApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<StudentReadDto>> GetAllAsync()
    {
        var students = await _httpClient.GetFromJsonAsync<IReadOnlyList<StudentReadDto>>("api/students");
        return students ?? [];
    }

    public async Task<StudentReadDto?> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/students/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<StudentReadDto>();
    }

    public async Task<(bool Success, string? ErrorMessage)> CreateAsync(StudentCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/students", dto);
        return await ToResultAsync(response);
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, StudentUpdateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/students/{id}", dto);
        return await ToResultAsync(response);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/students/{id}");
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
            return (false, "Student not found.");
        }

        var error = await response.Content.ReadFromJsonAsync<ApiError>();
        return (false, error?.ErrorMessage ?? "Student operation failed.");
    }

    private sealed class ApiError
    {
        public string? ErrorMessage { get; set; }
    }
}
