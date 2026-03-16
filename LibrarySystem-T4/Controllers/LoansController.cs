using Microsoft.AspNetCore.Mvc;
using LibrarySystem_T4.Models;

namespace LibrarySystem_T4.Controllers;

// Controller för att ta emot anrop samt kalla på loans-api

public class LoansController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory; // Private och readonly för att skydda och bevara variablernas värden, kan ej ändras efter deklaration.
    private readonly IConfiguration _config; // Läser konfigurationsvärden från user-secrets eller Azure

    public LoansController(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        // Privata fält för classen
        _httpClientFactory = httpClientFactory; // ASP.net Core genererad factory, sparas i variabeln
        _config = config; // Sparas för att kunna läsas i metoderna nedan
    }

    // GET-metod, visar lista på alla lån
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("API-Key", _config["ApiKey"]); // API-nyckel krävs av ApiKeyFilter i Loans API

        var loans = await client.GetFromJsonAsync<List<LoanViewModel>>(
            _config["ApiUrls:Loans"] + "/api/loans" // Hämtar alla lån från Loans API
        );

        return View(loans ?? new List<LoanViewModel>()); // Tom lista om API svarar null
    }

    // GET-metod för att visa detaljsidan för ett specifikt lån
    public async Task<IActionResult> Details(int id)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("API-Key", _config["ApiKey"]);

        var loan = await client.GetFromJsonAsync<LoanViewModel>(
            _config["ApiUrls:Loans"] + $"/api/loans/{id}" // Hämtar lån med matchande ID
        );

        if (loan == null) return NotFound(); // Returnerar 404 om lånet inte finns
        return View(loan);
    }

    // GET-metod för att skapa ett nytt lån
    public IActionResult Create(int itemId)
    {
        var model = new LoanViewModel { ItemId = itemId }; // ItemId fylls i från URL:en, t.ex. /Loans/Create?itemId=3
        return View(model);
    }

    // POST-metod för att skicka formulärdatan till loans-api när ett lån skapas
    [HttpPost]
    public async Task<IActionResult> Create(LoanViewModel model)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("API-Key", _config["ApiKey"]);

        var response = await client.PostAsJsonAsync(
            _config["ApiUrls:Loans"] + "/api/loans", model // Skickar formulärdata som JSON till Loans API
        );

        if (response.IsSuccessStatusCode) // Om Loans API svarar 201 Created
            return RedirectToAction("Index");

        return View(model); // Visa formuläret igen om något gick fel
    }

    // POST-metod för att återlämna lån
    [HttpPost]
    public async Task<IActionResult> Return(int id)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("API-Key", _config["ApiKey"]);

        await client.PutAsync(
            _config["ApiUrls:Loans"] + $"/api/loans/{id}/return", null // null i body, Loans API sätter ReturnedDate automatiskt
        );

        return RedirectToAction("Index");
    }

    // POST-metod för att radera ett lån
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("API-Key", _config["ApiKey"]);

        await client.DeleteAsync(
            _config["ApiUrls:Loans"] + $"/api/loans/{id}" // Skickar DELETE-anrop till Loans API
        );

        return RedirectToAction("Index"); // Omdirigera tillbaka till lånelistan efter radering
    }
}