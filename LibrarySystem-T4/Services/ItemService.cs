using LibrarySystem_T4.Models;

namespace LibrarySystem_T4.Services;

public class ItemService
{
    //  Denna används httpclient för att skicka HTTP-anrop till Items-API
    private readonly HttpClient _httpClient;

    //  Denna injicerar httpclient automatiskt via dependency injection
    public ItemService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Denna hämtar alla items från Item-api och lämnar tillbaka dem som en lista 
    public async Task<List<Item>> GetItemsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Item>>("api/items");
    }
}