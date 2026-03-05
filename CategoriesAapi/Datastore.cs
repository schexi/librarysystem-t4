using CategoriesApi.Models;

namespace CategoriesApi;

// Simple in-memory store — replace with a DbContext/SQLite later if needed
public class DataStore
{
    public List<Category> Categories { get; } = new()
    {
        new Category { Id = 1, Name = "Böcker",      Description = "Fysiska och digitala böcker" },
        new Category { Id = 2, Name = "Rapporter",   Description = "Interna rapporter och dokument" },
        new Category { Id = 3, Name = "Utrustning",  Description = "Teknisk utrustning för utlåning" },
    };

    public List<Item> Items { get; } = new()
    {
        new Item { Id = 1, CategoryId = 1, Title = "Clean Code",            Author = "Robert C. Martin", Type = "Book",      IsAvailable = true  },
        new Item { Id = 2, CategoryId = 1, Title = "The Pragmatic Programmer", Author = "Hunt & Thomas", Type = "Book",     IsAvailable = false },
        new Item { Id = 3, CategoryId = 2, Title = "Årsrapport 2023",       Author = "Kommunen",         Type = "Report",    IsAvailable = true  },
        new Item { Id = 4, CategoryId = 3, Title = "HDMI-kabel",            Author = "Utrustning",       Type = "Equipment", IsAvailable = true  },
        new Item { Id = 5, CategoryId = 3, Title = "Raspberry Pi 4",        Author = "Raspberry Pi Ltd", Type = "Equipment", IsAvailable = true  },
    };

    private int _nextCategoryId = 4;
    private int _nextItemId = 6;

    public int NextCategoryId() => _nextCategoryId++;
    public int NextItemId()     => _nextItemId++;
}