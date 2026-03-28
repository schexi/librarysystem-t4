using System.Net.Http.Json;

namespace LibrarySystem_T4.Services;

public class UserService
{
    private readonly HttpClient _httpClient;
    private const string UserApiUrl = "https://user-api-adde.azurewebsites.net";

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserProfileViewModel?> GetProfileAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return await _httpClient.GetFromJsonAsync<UserProfileViewModel>($"{UserApiUrl}/api/user/profile");
    }
}

public class UserProfileViewModel
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int ActiveLoans { get; set; }
}
