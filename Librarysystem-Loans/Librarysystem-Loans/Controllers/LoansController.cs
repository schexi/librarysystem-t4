using LibrarysystemLoans.Data;
using LibrarysystemLoans.KeyFilters;
using LibrarysystemLoans.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarysystemLoans.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(ApiKeyFilter))]
public class LoansController : ControllerBase
{
    private readonly LoansContext _context;

    public LoansController(LoansContext context)
    {
        _context = context;
    }

    // GET: api/loans - Hämtar alla lån med tillhörande status
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var loans = await _context.Loans
            .Include(l => l.LoanStatus) // Inkluderar statusobjektet via FK
            .ToListAsync();
        return Ok(loans);
    }

    // GET: api/loans/{id} - Hämtar ett specifikt lån via ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var loan = await _context.Loans
            .Include(l => l.LoanStatus)
            .FirstOrDefaultAsync(l => l.Id == id);
        if (loan == null) return NotFound();
        return Ok(loan);
    }

    // GET: api/loans/overdue - Hämtar alla försenade lån
    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdue()
    {
        var overdue = await _context.Loans
            .Include(l => l.LoanStatus)
            .Where(l => l.DueDate < DateTime.Now && l.ReturnedDate == null)
            .ToListAsync();
        return Ok(overdue);
    }

    // POST: api/loans - Skapar ett nytt lån
    [HttpPost]
    public async Task<IActionResult> Create(Loan loan)
    {
        loan.BorrowedDate = DateTime.Now; // Sätts automatiskt
        loan.LoanStatusId = 1; // Sätts till "Aktiv" som standard
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
    }

    // PUT: api/loans/{id}/return - Återlämnar ett lån
    [HttpPut("{id}/return")]
    public async Task<IActionResult> Return(int id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan == null) return NotFound();
        loan.ReturnedDate = DateTime.Now; // Sätts automatiskt vid återlämning
        loan.LoanStatusId = 3; // Uppdaterar status till "Återlämnad"
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // PUT: api/loans/{id}/status - Uppdaterar status på ett lån
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] int statusId)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan == null) return NotFound();
        var status = await _context.LoanStatuses.FindAsync(statusId);
        if (status == null) return BadRequest("Ogiltig status");
        loan.LoanStatusId = statusId; // Uppdaterar till vald status
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/loans/{id} - Raderar ett lån
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan == null) return NotFound();
        _context.Loans.Remove(loan);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}