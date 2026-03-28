using Microsoft.AspNetCore.Mvc;
using LibrarySystem_T4.Models;
using LibrarySystem_T4.Services;

namespace LibrarySystem_T4.Controllers;

// Controller för att ta emot anrop samt delegera till LoanService
public class LoansController : Controller
{
    private readonly LoanService _loanService; // Hanterar all kommunikation med Loans API
    private readonly IConfiguration _config; // Läser konfigurationsvärden från user-secrets eller Azure

    public LoansController(LoanService loanService, IConfiguration config)
    {
        _loanService = loanService;
        _config = config;
    }

    // GET-metod, visar lista på alla lån
    public async Task<IActionResult> Index()
    {
        var loans = await _loanService.GetAllAsync();
        return View(loans);
    }

    // GET-metod för att visa detaljsidan för ett specifikt lån
    public async Task<IActionResult> Details(int id)
    {
        var loan = await _loanService.GetByIdAsync(id);
        if (loan == null) return NotFound();
        return View(loan);
    }

    // GET-metod för att visa formulär för att skapa ett nytt lån
    public IActionResult Create(int itemId)
    {
        var model = new LoanViewModel
        {
            ItemId = itemId,
            DueDate = DateTime.Now.AddDays(28), // Förfallodatum sätts automatiskt 4 veckor framåt
            BorrowerName = "Admin Test", // TODO: Ska hämtas från Users API
            BorrowerEmail = "admin@mail.com" // TODO: Ska hämtas från Users API
        };
        return View(model);
    }

    // POST-metod för att skicka formulärdata till Loans API
    [HttpPost]
    public async Task<IActionResult> Create(LoanViewModel model)
    {
        var success = await _loanService.CreateAsync(model);
        if (success) return RedirectToAction("Index");
        return View(model); // Visa formuläret igen om något gick fel
    }

    // POST-metod för att återlämna lån
    [HttpPost]
    public async Task<IActionResult> Return(int id)
    {
        await _loanService.ReturnAsync(id);
        return RedirectToAction("Index");
    }

    // POST-metod för att radera ett lån
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _loanService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}