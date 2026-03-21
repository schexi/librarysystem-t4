namespace CategoriesApi.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
}

public class Item
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";   // e.g. book author or equipment brand
    public string Type { get; set; } = "";      // "Book", "Report", "Equipment"
    public bool IsAvailable { get; set; } = true;
}

// DTOs (what comes in from requests)
public record CreateCategoryRequest(string Name, string Description);
