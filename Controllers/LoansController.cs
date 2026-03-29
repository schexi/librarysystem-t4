using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LibrarySystem_T4.Data;
using LibrarySystem_T4.Models;
using LibrarySystem_T4.Models.Entities;
using LibrarySystem_T4.Services;

namespace LibrarySystem_T4.Controllers;

[ApiController]
[Route("api/loans")]
[Authorize]
public class LoansApiController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetUserLoans(int userId)
    {
        var requesterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin     = User.IsInRole("Admin");
        if (requesterId != userId && !isAdmin) return Forbid();

        var loans = await context.Loans
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.BorrowedDate)
            .ToListAsync();

        return Ok(loans);
    }

    [HttpPost("borrow")]
    public async Task<IActionResult> BorrowItem([FromBody] BorrowRequest req)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var existing = await context.Loans
            .AnyAsync(l => l.UserId == userId && l.ItemId == req.ItemId && !l.IsReturned);
        if (existing) return BadRequest("Du har redan lånat detta objekt.");

        var loan = new LoanEntity
        {
            UserId       = userId,
            ItemId       = req.ItemId,
            BookTitle    = req.Title  ?? "Okänd titel",
            BookAuthor   = req.Author ?? "",
            BorrowedDate = DateTime.UtcNow,
            DueDate      = DateTime.UtcNow.AddDays(28),
            IsReturned   = false
        };

        context.Loans.Add(loan);
        await context.SaveChangesAsync();
        return Ok(loan);
    }

    [HttpPost("{id:int}/return")]
    public async Task<IActionResult> ReturnLoan(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var loan   = await context.Loans.FindAsync(id);
        if (loan == null) return NotFound();
        if (loan.UserId != userId && !User.IsInRole("Admin")) return Forbid();

        loan.IsReturned   = true;
        loan.ReturnedDate = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Ok(new { message = "Lån återlämnat." });
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteLoan(int id)
    {
        var loan = await context.Loans.FindAsync(id);
        if (loan == null) return NotFound();
        context.Loans.Remove(loan);
        await context.SaveChangesAsync();
        return Ok(new { message = "Lån raderat." });
    }

    [HttpGet("overdue")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOverdue()
    {
        var loans = await context.Loans
            .Where(l => !l.IsReturned && l.DueDate < DateTime.UtcNow)
            .Include(l => l.User)
            .ToListAsync();
        return Ok(loans);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var loans = await context.Loans
            .Include(l => l.User)
            .OrderByDescending(l => l.BorrowedDate)
            .ToListAsync();
        return Ok(loans);
    }
}

public class BorrowRequest
{
    public int     ItemId { get; set; }
    public int     UserId { get; set; }
    public string? Title  { get; set; }
    public string? Author { get; set; }
}

public class LoansController : Controller
{
    private readonly LoanService _loanService;
    public LoansController(LoanService loanService) { _loanService = loanService; }

    public async Task<IActionResult> Index()
    {
        var loans = await _loanService.GetAllAsync();
        return View(loans);
    }

    public async Task<IActionResult> Details(int id)
    {
        var loan = await _loanService.GetByIdAsync(id);
        if (loan == null) return NotFound();
        return View(loan);
    }

    [HttpPost]
    public async Task<IActionResult> Return(int id)
    {
        await _loanService.ReturnAsync(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _loanService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}