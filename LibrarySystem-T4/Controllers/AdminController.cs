using Microsoft.AspNetCore.Mvc;
using LibrarySystem_T4.Models;
using LibrarySystem_T4.Services;

namespace LibrarySystem_T4.Controllers;

public class AdminController : Controller
{
    private readonly LoanService _loanService;
    private readonly IConfiguration _config;

    public AdminController(LoanService loanService, IConfiguration config)
    {
        _loanService = loanService;
        _config = config;
    }

    public async Task<IActionResult> Loans()
    {
        var loans = await _loanService.GetAllAsync();
        return View(loans);
    }

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

    [HttpPost]
    public async Task<IActionResult> Create(LoanViewModel model)
    {
        var success = await _loanService.CreateAsync(model);
        if (success) return RedirectToAction("Loans");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Return(int id)
    {
    {
blic async Task<IActionResult> Reid);
        return RedirectToAction("Loans");
    }    }    }    }    }    }    }    }    IActionResult> Delete(int id)
    {
        await _loanService.DeleteAsync(id);
        return RedirectToAction("Loans");
    }
}
