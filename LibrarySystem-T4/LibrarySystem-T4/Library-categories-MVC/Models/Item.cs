namespace LibrarySystem_T4.Models;

public class Item
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Type { get; set; } = "";
    public bool IsAvailable { get; set; }
}