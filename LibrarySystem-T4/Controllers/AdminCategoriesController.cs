using Microsoft.AspNetCore.Mvc;
using LibrarySystem_T4.Models;
using LibrarySystem_T4.Services;

namespace LibrarySystem_T4.Controllers;

// Den här controllern är till för admin-delen av kategorier.
// Tanken är att admin ska kunna göra CRUD:
// Create, Read, Update, Delete
public class AdminCategoriesController : Controller
{
    // Service som pratar med Categories API
    private readonly CategoryService _service;

    // Konstruktorn körs när controllern skapas.
    // ASP.NET skickar automatiskt in CategoryService via dependency injection.
    public AdminCategoriesController(CategoryService service)
    {
        _service = service;
    }

    
    // GET: /AdminCategories
    // Visar alla kategorier i adminvyn
    // Här ska admin kunna se listan och få knappar för edit/delete
    
    public async Task<IActionResult> Index()
    {
        // Hämtar alla kategorier från API:t via servicen
        var categories = await _service.GetAllAsync();

        // Skickar listan vidare till vyn
        return View(categories);
    }

    
    // GET: /AdminCategories/Create
    // Visar formuläret för att skapa en ny kategori
    // Inget sparas här, vi visar bara sidan
    
    public IActionResult Create()
    {
        return View();
    }

    
    // POST: /AdminCategories/Create
    // Tar emot formulärdata när admin skickar in ny kategori
    
    [HttpPost]
    public async Task<IActionResult> Create(CategoryViewModel model)
    {
        // Kontrollerar om modellen är giltig
        // Om något saknas eller är fel, visas samma formulär igen
        if (!ModelState.IsValid) return View(model);

        // Skickar kategorin till API:t för att skapa den
        await _service.CreateAsync(model);

        // När skapandet lyckas skickas admin tillbaka till listan
        return RedirectToAction("Index");
    }

    
    // GET: /AdminCategories/Edit/5
    // Hämtar en viss kategori och visar edit-formuläret
    
    public async Task<IActionResult> Edit(int id)
    {
        // Hämtar kategorin med valt id från API:t
        var category = await _service.GetByIdAsync(id);

        // Om kategorin inte finns returneras 404
        if (category == null) return NotFound();

        // Skickar kategorin till edit-vyn så fälten kan fyllas i
        return View(category);
    }

    
    // POST: /AdminCategories/Edit
    // Tar emot ändrad kategori från edit-formuläret
    
    [HttpPost]
    public async Task<IActionResult> Edit(CategoryViewModel model)
    {
        // Om modellen inte är giltig visas formuläret igen
        if (!ModelState.IsValid) return View(model);

        // Uppdaterar kategorin i API:t med hjälp av id
        await _service.UpdateAsync(model.Id, model);

        // När uppdateringen lyckas går admin tillbaka till listan
        return RedirectToAction("Index");
    }

    
    // POST: /AdminCategories/Delete
    // Tar bort en kategori
    // Vi använder POST för att inte råka radera via vanlig länk
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        // Skickar delete-anrop till API:t
        await _service.DeleteAsync(id);

        // När borttagningen är klar går admin tillbaka till listan
        return RedirectToAction("Index");
    }
}