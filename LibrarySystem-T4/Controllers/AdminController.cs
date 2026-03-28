using Microsoft.AspNetCore.Mvc;
using LibrarySystem_T4.Models;
using LibrarySystem_T4.Services;

namespace LibrarySystem_T4.Controllers;

// Controller för adminvyn - hanterar alla lån som administratör
public class AdminController : Controller
{
    private readonly LoanService _loanService;
    private readonly IConfiguration _config;

    public AdminController(LoanService loanService, IConfiguration config)
    {
        _loanService = loanService;
        _config = config;
    }

    // GET: /Admin/Loans - Visar alla lån för admin
    public async Task<IActionResult> Loans()
    {
        var loans = await _loanService.GetAllAsync();
        return View(loans);
    }

    // GET: /Admin/Create - Visar formulär för att skapa lån
    public IActionResult Create(int itemId = 0)
    {
        var model = new LoanViewModel
        {
            ItemId = itemId,
            DueDate = DateTime.Now.AddDays(28),
            BorrowerName = "",
            BorrowerEmail = ""
        };
        return View(model);
    }

    // POST: /Admin/Create - Skapar ett nytt lån
    [HttpPost]
    public async Task<IActionResult> Create(LoanViewModel model)
    {
        var success = await _loanService.CreateAsync(model);
        if (success) return RedirectToAction("Loans");
        return View(model);
    }

    // POST: /Admin/Return - Återlämnar ett lån
    [HttpPost]
    public async Task<IActionResult> Return(int id)
    {
        await _loanService.ReturnAsync(id);
        return RedirectToAction("Loans");
    }

    // POST: /Admin/Delete - Raderar ett lån
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _loanService.DeleteAsync(id);
        return RedirectToAction("Loans");
    }
    
    
}