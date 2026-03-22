using Loans_API.KeyFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoansApi.Data;
using LoansApi.Models;

namespace LoansApi.Controllers;

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

    // GET: api/loans - Hämtar alla lån
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var loans = await _context.Loans.ToListAsync();
        return Ok(loans);
    }

    // GET: api/loans/{id} - Hämtar ett specifikt lån via ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan == null) return NotFound();
        return Ok(loan);
    }

    // GET: api/loans/overdue - Hämtar alla försenade lån (förfallna och ej återlämnade)
    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdue()
    {
        var overdue = await _context.Loans
            .Where(l => l.DueDate < DateTime.Now && l.ReturnedDate == null)
            .ToListAsync();
        return Ok(overdue);
    }

    // POST: api/loans - Skapar ett nytt lån
    [HttpPost]
    public async Task<IActionResult> Create(Loan loan)
    {
        loan.BorrowedDate = DateTime.Now; // Sätts automatiskt
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
    }

    // PUT: api/loans/{id}/return - Återlämnar ett lån, sätter ReturnedDate
    [HttpPut("{id}/return")]
    public async Task<IActionResult> Return(int id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan == null) return NotFound();
        loan.ReturnedDate = DateTime.Now; // Lånet markeras som återlämnat
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