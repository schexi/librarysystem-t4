/*using Microsoft.AspNetCore.Mvc;
using LibrarySystem_T4.Models;
using LibrarySystem_T4.Services;
using Microsoft.AspNetCore.Authorization;

namespace LibrarySystem_T4.Controllers;

public class CategoriesController : Controller
{
    private readonly CategoryService _service;

    public CategoriesController(CategoryService service)
    {
        _service = service;
    }

    // GET /Categories
    public async Task<IActionResult> Index()
    {
        var categories = await _service.GetAllAsync();
        return View(categories);
    }

    // GET /Categories/Create

    public IActionResult Create()
    {
        return View();
    }

    // POST /Categories/Create

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        await _service.CreateAsync(category);
        return RedirectToAction(nameof(Index));
    }

    // GET /Categories/Edit/1
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _service.GetByIdAsync(id);
        if (category is null) return NotFound();
        return View(category);
    }

    // POST /Categories/Edit/1
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        await _service.UpdateAsync(id, category);
        return RedirectToAction(nameof(Index));
    }

    // POST /Categories/Delete/1
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}*/


using Microsoft.AspNetCore.Mvc;
using LibrarySystem_T4.Services;

namespace LibrarySystem_T4.Controllers;

public class CategoriesController : Controller
{
    private readonly CategoryService _categoryService;

    public CategoriesController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // Vanlig användare: ser bara listan
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();
        return View(categories);
    }

    // Vanlig användare: kan se detaljer om du vill ha det
    public async Task<IActionResult> Details(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null) return NotFound();

        return View(category);
    }
}

