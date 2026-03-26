using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Library.User.Api.Data;

namespace Library.User.Api.Controllers;

[ApiController]
[Route("api/loans")]
[Authorize]
public class LoansController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetUserLoans(int userId)
    {
        var requesterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin     = User.IsInRole("Admin");

        if (requesterId != userId && !isAdmin)
            return Forbid();

        var loans = await context.Loans
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.BorrowedDate)
            .ToListAsync();

        return Ok(loans);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetMyActiveLoans()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var loans  = await context.Loans
            .Where(l => l.UserId == userId && !l.IsReturned)
            .OrderBy(l => l.DueDate)
            .ToListAsync();

        return Ok(loans);
    }

    [HttpGet("overdue")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOverdueLoans()
    {
        var now    = DateTime.UtcNow;
        var loans  = await context.Loans
            .Where(l => !l.IsReturned && l.DueDate < now)
            .Include(l => l.User)
            .OrderBy(l => l.DueDate)
            .ToListAsync();

        return Ok(loans);
    }
}