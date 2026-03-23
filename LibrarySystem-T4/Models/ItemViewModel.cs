namespace LibrarySystem_T4.Models;

// Model för att representera ett objekt från Items API i ett lån
public class ItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}