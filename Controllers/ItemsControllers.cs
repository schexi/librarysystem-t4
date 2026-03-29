using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using LibrarySystem_T4.Models;

namespace LibrarySystem_T4.Controllers;

public class ItemsController : Controller
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl         = "https://items-api-adcac3a6hndtc0c5.norwayeast-01.azurewebsites.net/api/items";
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
        catch (Exception ex) { return Content(ex.Message); }
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>(CategoryApiUrl);
            ViewBag.Categories = categories ?? new();
        }
        catch
        {
            ViewBag.Categories = new List<CategoryViewModel>();
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ItemViewModel item)
    {
        item.Id          = 0;
        item.IsAvailable = true;
        item.AddedDate   = DateTime.UtcNow;

        var response = await _httpClient.PostAsJsonAsync(ApiUrl, item);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return Content($"Fel från Items-API: {response.StatusCode} — {error}");
        }

        var created = await response.Content.ReadFromJsonAsync<ItemViewModel>();
        if (created != null && (!string.IsNullOrEmpty(item.Author) || item.PublishYear.HasValue))
        {
            await _httpClient.PostAsJsonAsync(
                "https://t4bibliotek.azurewebsites.net/api/bookmeta",
                new { itemId = created.Id, author = item.Author ?? "", publishYear = item.PublishYear, totalCopies = item.TotalCopies }
            );
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var item       = await _httpClient.GetFromJsonAsync<ItemViewModel>($"{ApiUrl}/{id}");
        var categories = await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>(CategoryApiUrl);
        ViewBag.Categories = categories ?? new();
        return View(item);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, ItemViewModel item)
    {
        var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}/{id}", item);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return Content($"Fel från Items-API: {response.StatusCode} — {error}");
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
        return RedirectToAction(nameof(Index));
    }
}