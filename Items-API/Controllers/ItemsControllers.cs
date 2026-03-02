using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Items_API.Data;
using Items_API.Models;

namespace Items_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsControllers : ControllerBase
{
    // Denna är databaskopplingen för metoderna
    private readonly AppDbContext _context;
    
    // Denna tar emot databas
    public ItemsControllers(AppDbContext context)
    {
        _context = context;
    }
    
    // Denna hämtar alla objekt från databas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> GetItems()
    {
        return await _context.Items.ToListAsync();
    }

    // Denna hämtar ett specifikt objekt efter id, om den inte finns returnerar en 404 error
    [HttpGet("{id}")]
    public async Task<ActionResult<Item>> GetItem(int id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item == null) return NotFound();
        return item;
    }
    
    // Denna skapar ett nytt objekt sedan sparar det i databasen
    [HttpPost]
    public async Task<ActionResult<Item>> PostItem(Item item)
    {
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
    }
    
    // Denna uppdaterar ett befintligt objekt, om den inte finns returnerar en 400 error om id inte matchar
    [HttpPut("{id}")]
    public async Task<IActionResult> PutItem(int id, Item item)
    {
        if (id != item.Id) return BadRequest();
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    // Denna tar bort ett objekt med id, om den inte finns returnerar en 404 error
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item == null) return NotFound();
        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}