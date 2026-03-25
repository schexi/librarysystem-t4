using LibrarySystemFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LibrarySystemFrontend.Controllers;

public class NotificationsController : Controller
{
    private readonly HttpClient _httpClient;

    public NotificationsController()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5165/");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _httpClient.GetAsync("api/notifications");

        if (!response.IsSuccessStatusCode)
        {
            return View(new List<Notification>());
        }

        var json = await response.Content.ReadAsStringAsync();

        var notifications = JsonSerializer.Deserialize<List<Notification>>(json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Notification>();

        return View(notifications);
    }
}¨