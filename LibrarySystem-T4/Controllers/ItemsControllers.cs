using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using LibrarySystem_T4.Models;

namespace LibrarySystem_T4.Controllers;

public class ItemsController : Controller
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://items-api-adcac3a6hndtc0c5.norwayeast-01.azurewebsites.net/api/items";
    private const string CategoryApiUrl = "https://kategori-cbc6adfyhwafa3fd.norwayeast-01.azurewebsites.net/api/categories";

    public ItemsController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("ItemsApi");
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var items = await _httpClient.GetFromJsonAsync<List<ItemViewModel>>(ApiUrl);
            return View(items);
        }
        catch (Exception ex)
        {
            return Content(ex.Message);
        }
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>(CategoryApiUrl);
        ViewBag.Categories = categories ?? new List<CategoryViewModel>();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ItemViewModel item)
    {
        item.Id = 0;
        await _httpClient.PostAsJsonAsync(ApiUrl, item);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _httpClient.GetFromJsonAsync<ItemViewModel>($"{ApiUrl}/{id}");
        var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>(CategoryApiUrl);
        ViewBag.Categories = categories ?? new List<CategoryViewModel>();
        return View(item);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, ItemViewModel item)
    {
        await _httpClient.PutAsJsonAsync($"{ApiUrl}/{id}", item);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
        return RedirectToAction("Index");
    }
}

public class ItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
}

