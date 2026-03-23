using LibrarySystem_T4.Models;

namespace LibrarySystem_T4.Services;

// Hanterar all kommunikation med Loans API, Items API och User API
public class LoanService
{
    private readonly HttpClient _client; // Dedikerad klient för Loans API med API-nyckel
    private readonly IHttpClientFactory _httpClientFactory; // Factory för anrop till Items och User API
    private readonly IConfiguration _config;

    public LoanService(HttpClient client, IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _client = client;
        _httpClientFactory = httpClientFactory;
        _config = config;
        _client.DefaultRequestHeaders.Add("API-Key", _config["ApiKey"]); // API-nyckel läggs till en gång för alla Loans-anrop
    }

    // Hämtar alla lån från Loans API och berikar med ItemTitle från Items API
    public async Task<List<LoanViewModel>> GetAllAsync()
    {
        var loans = await _client.GetFromJsonAsync<List<LoanViewModel>>(
            _config["ApiUrls:Loans"] + "/api/loans"
        );

        if (loans == null) return new List<LoanViewModel>();

        // Försöker hämta itemnamn från Items API för varje lån
        foreach (var loan in loans)
        {
            try
            {
                var item = await _httpClientFactory.CreateClient()
                    .GetFromJsonAsync<ItemViewModel>(
                        _config["ApiUrls:Items"] + $"/api/items/{loan.ItemId}"
                    );
                if (item != null)
                    loan.ItemTitle = item.Title; // Sätter titeln om Items API svarar
            }
            catch
            {
                loan.ItemTitle = $"Objekt #{loan.ItemId}"; // Fallback om Items API inte svarar
            }
        }

        return loans;
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

    // Hämtar alla objekt från Items API — returnerar tom lista om API inte svarar
    public async Task<List<ItemViewModel>> GetAllItemsAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var items = await client.GetFromJsonAsync<List<ItemViewModel>>(
                _config["ApiUrls:Items"] + "/api/items"
            );
            return items ?? new List<ItemViewModel>();
        }
        catch
        {
            return new List<ItemViewModel>(); // TODO: Items API inte tillgängligt än
        }
    }

    // Hämtar alla användare från User API — returnerar tom lista om API inte svarar
    public async Task<List<UserViewModel>> GetAllUsersAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var users = await client.GetFromJsonAsync<List<UserViewModel>>(
                _config["ApiUrls:Users"] + "/api/users"
            );
            return users ?? new List<UserViewModel>();
        }
        catch
        {
            return new List<UserViewModel>(); // TODO: User API inte tillgängligt än
        }
    }
}