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
    public async Task<List<CategoryViewModel>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<CategoryViewModel>>("api/categories") ?? new();
    }

    // Hämtar en specifik kategori
    public async Task<CategoryViewModel?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<CategoryViewModel>($"api/categories/{id}");
    }

    // Skapar en ny kategori
    public async Task CreateAsync(CategoryViewModel categoryViewModel)
    {
        await _http.PostAsJsonAsync("api/categories", categoryViewModel);
    }

    // Uppdaterar en kategori
    public async Task UpdateAsync(int id, CategoryViewModel categoryViewModel)
    {
        await _http.PutAsJsonAsync($"api/categories/{id}", categoryViewModel);
    }

    // Tar bort en kategori
    public async Task DeleteAsync(int id)
    {
        await _http.DeleteAsync($"api/categories/{id}");
    }
}