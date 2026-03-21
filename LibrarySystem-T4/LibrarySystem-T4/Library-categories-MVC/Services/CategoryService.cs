using LibrarySystem_T4.Models;

namespace LibrarySystem_T4.Services;

public class CategoryService
{
    private readonly HttpClient _http;

    public CategoryService(HttpClient http)
    {
        _http = http;
    }

    // Hämtar alla kategorier från API:et
    public async Task<List<Category>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<Category>>("api/categories") ?? new();
    }

    // Hämtar en specifik kategori
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<Category>($"api/categories/{id}");
    }

    // Skapar en ny kategori
    public async Task CreateAsync(Category category)
    {
        await _http.PostAsJsonAsync("api/categories", category);
    }

    // Uppdaterar en kategori
    public async Task UpdateAsync(int id, Category category)
    {
        await _http.PutAsJsonAsync($"api/categories/{id}", category);
    }

    // Tar bort en kategori
    public async Task DeleteAsync(int id)
    {
        await _http.DeleteAsync($"api/categories/{id}");
    }
    
    // Hämtar alla items i en kategori via ditt API
    public async Task<List<Item>> GetItemsAsync(int categoryId)
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Item>>($"api/categories/{categoryId}/items") ?? new();
        }
        catch
        {
            // items-api är inte igång — returnera tom lista
            return new List<Item>();
        }
    }
    
}