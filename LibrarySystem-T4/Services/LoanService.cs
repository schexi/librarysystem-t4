using LibrarySystem_T4.Models;

namespace LibrarySystem_T4.Services;

// Hanterar all kommunikation med Loans API
public class LoanService
{
    private readonly HttpClient _client;
    private readonly IConfiguration _config;

    public LoanService(HttpClient client, IConfiguration config)
    {
        _client = client;
        _config = config;
        _client.DefaultRequestHeaders.Add("API-Key", _config["ApiKey"]); // API-nyckel läggs till en gång för alla anrop
    }

    // Hämtar alla lån från Loans API
    public async Task<List<LoanViewModel>> GetAllAsync()
    {
        var loans = await _client.GetFromJsonAsync<List<LoanViewModel>>(
            _config["ApiUrls:Loans"] + "/api/loans"
        );
        return loans ?? new List<LoanViewModel>();
    }

    // Hämtar ett specifikt lån via ID
    public async Task<LoanViewModel?> GetByIdAsync(int id)
    {
        return await _client.GetFromJsonAsync<LoanViewModel>(
            _config["ApiUrls:Loans"] + $"/api/loans/{id}"
        );
    }

    // Skapar ett nytt lån
    public async Task<bool> CreateAsync(LoanViewModel model)
    {
        var response = await _client.PostAsJsonAsync(
            _config["ApiUrls:Loans"] + "/api/loans", model
        );
        return response.IsSuccessStatusCode; // Returnerar true om 201 Created
    }

    // Återlämnar ett lån
    public async Task ReturnAsync(int id)
    {
        await _client.PutAsync(
            _config["ApiUrls:Loans"] + $"/api/loans/{id}/return", null
        );
    }

    // Raderar ett lån
    public async Task DeleteAsync(int id)
    {
        await _client.DeleteAsync(
            _config["ApiUrls:Loans"] + $"/api/loans/{id}"
        );
    }
}