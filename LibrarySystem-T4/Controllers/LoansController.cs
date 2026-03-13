using Microsoft.AspNetCore.Mvc;
using LibrarySystem_T4.Models;

namespace LibrarySystem_T4.Controllers;

// Controller för att ta emot anrop samt kalla på loans-api

public class LoansController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory; // Private och readonly för att skydda och bevara variablernas värden, kan ej ändras efter deklaration.
    private readonly IConfiguration _config;

    public LoansController(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        // Privata fält för classen
        _httpClientFactory = httpClientFactory; // ASP.net Core genererad factory, sparas i variabeln
        _config = config;
    }

    // GET-metod, visar lista på alla lån
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("API-Key", _config["ApiKey"]);

        var loans = await client.GetFromJsonAsync<List<LoanViewModel>>(
            _config["ApiUrls:Loans"] + "/api/loans"
        );

        return View(loans ?? new List<LoanViewModel>());
    }

    // GET-metod för att skapa ett nytt lån
    public IActionResult Create(int itemId)
    {
        var model = new LoanViewModel { ItemId = itemId };
        return View(model);
    }

    // POST-metod för att skicka formulärdatan till loans-api när ett lån skapas
    [HttpPost]
    public async Task<IActionResult> Create(LoanViewModel model)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("API-Key", _config["ApiKey"]);

        var response = await client.PostAsJsonAsync(
            _config["ApiUrls:Loans"] + "/api/loans", model
        );

        if (response.IsSuccessStatusCode)
            return RedirectToAction("Index");

        return View(model);
    }

    // POST-metod för att återlämna lån
    [HttpPost]
    public async Task<IActionResult> Return(int id)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("API-Key", _config["ApiKey"]);

        await client.PutAsync(
            _config["ApiUrls:Loans"] + $"/api/loans/{id}/return", null
        );

        return RedirectToAction("Index");
    }
}