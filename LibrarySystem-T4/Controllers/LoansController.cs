using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem_T4.Controllers;

// Class för att ta emot anrop från användaraktivitet, samt anropa Loans-API

public class LoansController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory; // Skapa HTTPClient objekt som anropar Loans-API:n
    private readonly IConfiguration _config;   // Läser värden från appsettings
}