using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kategori_Api.Data;
using Kategori_Api.Models;

namespace Kategori_Api.Controllers;


[ApiController]

// Bestämmer URL:en → api/categories
[Route("api/categories")]
public class KategoriController : ControllerBase
{
    // Koppling till databasen
    private readonly AppDbContext _context;

   

    // Konstruktor – dependency injection
    // ASP.NET skickar automatiskt in dessa
    public KategoriController(AppDbContext context)
    {
        _context = context;
    }

   
    // GET ALLA KATEGORIER
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Kategori>>> GetKategorier()
    {
        // Hämtar alla kategorier från databasen
        return await _context.Kategorier.ToListAsync();
    }

    
    // GET EN KATEGORI MED ID
   
    [HttpGet("{id}")]
    public async Task<ActionResult<Kategori>> GetKategori(int id)
    {
        // Letar efter kategori i databasen
        var kategori = await _context.Kategorier.FindAsync(id);

        // Om den inte finns → returnera 404
        if (kategori == null) return NotFound();

        // Annars returnera kategorin
        return kategori;
    }

 
    // SKAPA NY KATEGORI
   
    [HttpPost]
    public async Task<ActionResult<Kategori>> PostKategori(Kategori kategori)
    {
        // Lägger till i databasen
        _context.Kategorier.Add(kategori);

        // Sparar ändringarna
        await _context.SaveChangesAsync();

        // Returnerar 201 Created + location header
        return CreatedAtAction(nameof(GetKategori), new { id = kategori.Id }, kategori);
    }

   
    // UPPDATERA KATEGORI
   
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutKategori(int id, Kategori kategori)
    {
        // Säkerställer att URL id matchar objektets id
        if (id != kategori.Id) return BadRequest();

        // Markerar objektet som uppdaterat
        _context.Entry(kategori).State = EntityState.Modified;

        // Sparar ändringar
        await _context.SaveChangesAsync();

        return NoContent(); // 204
    }

   
    // DELETE KATEGORI
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteKategori(int id)
    {
        // Hittar kategori
        var kategori = await _context.Kategorier.FindAsync(id);

        // Om den inte finns → 404
        if (kategori == null) return NotFound();

        // Tar bort den från databasen
        _context.Kategorier.Remove(kategori);

        // Sparar
        await _context.SaveChangesAsync();

        return NoContent(); // 204
    }

   
}