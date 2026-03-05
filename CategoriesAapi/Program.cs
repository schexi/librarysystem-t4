using CategoriesApi;
using CategoriesApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DataStore as a singleton (shared in-memory state)
builder.Services.AddSingleton<DataStore>();

// Allow other services in your group to call this API
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

// ─────────────────────────────────────────────
//  CATEGORIES
// ─────────────────────────────────────────────

// GET /categories
// Returns all categories
app.MapGet("/categories", (DataStore db) => db.Categories);

// GET /categories/{id}
// Returns a single category, or 404
app.MapGet("/categories/{id:int}", (int id, DataStore db) =>
{
    var category = db.Categories.FirstOrDefault(c => c.Id == id);
    return category is not null ? Results.Ok(category) : Results.NotFound();
});

// POST /categories
// Body: { "name": "Böcker", "description": "..." }
app.MapPost("/categories", (CreateCategoryRequest req, DataStore db) =>
{
    var category = new Category
    {
        Id          = db.NextCategoryId(),
        Name        = req.Name,
        Description = req.Description
    };
    db.Categories.Add(category);
    return Results.Created($"/categories/{category.Id}", category);
});

// DELETE /categories/{id}
app.MapDelete("/categories/{id:int}", (int id, DataStore db) =>
{
    var category = db.Categories.FirstOrDefault(c => c.Id == id);
    if (category is null) return Results.NotFound();

    db.Categories.Remove(category);
    // Also remove all items belonging to this category
    db.Items.RemoveAll(i => i.CategoryId == id);
    return Results.NoContent();
});

// ─────────────────────────────────────────────
//  ITEMS WITHIN A CATEGORY
// ─────────────────────────────────────────────

// GET /categories/{id}/items?sort=title|author|type&available=true
// Returns items in a category, with optional sorting and availability filter
app.MapGet("/categories/{id:int}/items", (int id, string? sort, bool? available, DataStore db) =>
{
    if (!db.Categories.Any(c => c.Id == id))
        return Results.NotFound(new { error = $"Category {id} not found" });

    IEnumerable<Item> items = db.Items.Where(i => i.CategoryId == id);

    // Filter by availability if requested
    if (available.HasValue)
        items = items.Where(i => i.IsAvailable == available.Value);

    // Sort
    items = sort?.ToLower() switch
    {
        "author" => items.OrderBy(i => i.Author),
        "type"   => items.OrderBy(i => i.Type),
        _        => items.OrderBy(i => i.Title)   // default: sort by title
    };

    return Results.Ok(items.ToList());
});

// POST /categories/{id}/items
// Adds a new item to a category
// Body: { "title": "...", "author": "...", "type": "Book|Report|Equipment" }
app.MapPost("/categories/{id:int}/items", (int id, CreateItemRequest req, DataStore db) =>
{
    if (!db.Categories.Any(c => c.Id == id))
        return Results.NotFound(new { error = $"Category {id} not found" });

    var item = new Item
    {
        Id          = db.NextItemId(),
        CategoryId  = id,
        Title       = req.Title,
        Author      = req.Author,
        Type        = req.Type,
        IsAvailable = true
    };
    db.Items.Add(item);
    return Results.Created($"/categories/{id}/items/{item.Id}", item);
});

// DELETE /categories/{categoryId}/items/{itemId}
app.MapDelete("/categories/{categoryId:int}/items/{itemId:int}", (int categoryId, int itemId, DataStore db) =>
{
    var item = db.Items.FirstOrDefault(i => i.Id == itemId && i.CategoryId == categoryId);
    if (item is null) return Results.NotFound();

    db.Items.Remove(item);
    return Results.NoContent();
});

// ─────────────────────────────────────────────
//  ALL ITEMS (used by other services in your group)
// ─────────────────────────────────────────────

// GET /items
// Returns every item across all categories — useful for the loans service etc.
app.MapGet("/items", (DataStore db) => db.Items);

// GET /items/{id}
app.MapGet("/items/{id:int}", (int id, DataStore db) =>
{
    var item = db.Items.FirstOrDefault(i => i.Id == id);
    return item is not null ? Results.Ok(item) : Results.NotFound();
});

// PATCH /items/{id}/availability
// Called by the loans service when an item is borrowed or returned
// Body: { "isAvailable": false }
app.MapPatch("/items/{id:int}/availability", (int id, AvailabilityRequest req, DataStore db) =>
{
    var item = db.Items.FirstOrDefault(i => i.Id == id);
    if (item is null) return Results.NotFound();

    item.IsAvailable = req.IsAvailable;
    return Results.Ok(item);
});

app.Run();

// Used for PATCH body
record AvailabilityRequest(bool IsAvailable);