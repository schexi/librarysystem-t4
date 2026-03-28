using System.Net.Http.Json;

namespace LibrarySystem_T4.Services;

public class UserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserDto>?> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<UserDto>>("api/user");
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<UserDto>($"api/user/{id}");
    }
}

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}