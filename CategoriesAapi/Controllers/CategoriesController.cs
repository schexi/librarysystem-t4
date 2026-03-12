using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CategoriesApi.Models;

namespace CategoriesAapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;

    // Konstruktor — databasen och HttpClient injiceras automatiskt
    public CategoriesController(AppDbContext db, IHttpClientFactory httpClientFactory)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
    }

    // GET api/categories
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _db.Categories.ToListAsync();
        return Ok(categories);
    }

    // GET api/categories/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        return category is not null ? Ok(category) : NotFound();
    }

    // POST api/categories
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryRequest req)
    {
        var category = new Category
        {
            Name        = req.Name,
            Description = req.Description
        };
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
    }

    // PUT api/categories/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateCategoryRequest req)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category is null) return NotFound();

        category.Name        = req.Name;
        category.Description = req.Description;
        await _db.SaveChangesAsync();
        return Ok(category);
    }

    // DELETE api/categories/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category is null) return NotFound();

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET api/categories/1/items
    [HttpGet("{id}/items")]
    public async Task<IActionResult> GetItems(int id, [FromQuery] string? sort, [FromQuery] bool? available)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category is null)
            return NotFound(new { error = $"Category {id} not found" });

        // Anropar items-api och hämtar alla items
        var client = _httpClientFactory.CreateClient("ItemsApi");
        var items = await client.GetFromJsonAsync<List<Item>>("api/items");

        if (items is null)
            return Problem("Kunde inte nå items-api");

        // Filtrerar lokalt på categoryId
        IEnumerable<Item> filtered = items.Where(i => i.CategoryId == id);

        if (available.HasValue)
            filtered = filtered.Where(i => i.IsAvailable == available.Value);

        filtered = sort?.ToLower() switch
        {
            "author" => filtered.OrderBy(i => i.Author),
            "type"   => filtered.OrderBy(i => i.Type),
            _        => filtered.OrderBy(i => i.Title)
        };

        return Ok(filtered.ToList());
    }
}
