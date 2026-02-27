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

    
    // REST HTTP - metoder
    
    // GET- metod för att hämta alla lån
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var loans = await _context.Loans.ToListAsync();
        return Ok(loans);
    }

    // GET- metod för att hämta specifika lån via ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan == null) return NotFound();
        return Ok(loan);
    }

    // GET-metod för att hämta alla försenade lån
    [HttpGet("overdue")]
    public async Task<IActionResult> GetOverdue()
    {
        var overdue = await _context.Loans
            .Where(l => l.DueDate < DateTime.Now && l.ReturnedDate == null)
            .ToListAsync();
        return Ok(overdue);
    }

    // POST- metod för att skapa ett nytt lån
    [HttpPost]
    public async Task<IActionResult> Create(Loan loan)
    {
        loan.BorrowedDate = DateTime.Now;
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync(); // Skriver in det nya lånet i loans.db databasen
        return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
    }

    // PUT- metod för att återlämna lån
    [HttpPut("{id}/return")]
    public async Task<IActionResult> Return(int id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan == null) return NotFound();
        loan.ReturnedDate = DateTime.Now;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Radera ett lån
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