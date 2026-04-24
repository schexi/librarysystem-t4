using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using LibrarySystem_T4.Models;

namespace LibrarySystem_T4.Controllers;

public class NotificationsController : Controller
{
    private readonly HttpClient _httpClient;

    public NotificationsController()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://notifications-api-edin.azurewebsites.net/");
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
}