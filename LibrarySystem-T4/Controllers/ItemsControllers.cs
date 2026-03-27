using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace LibrarySystem_T4.Controllers;

public class ItemsController : Controller
{
    private readonly HttpClient _httpClient;

    public ItemsController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ItemsApi");
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var items = await _httpClient.GetFromJsonAsync<List<ItemViewModel>>("https://items-api-adcac3a6hndtc0c5.norwayeast-01.azurewebsites.net/api/items");
            return View(items);
        }
        catch (Exception ex)
        {
            return Content(ex.Message);
        }
    }
}

public class ItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}