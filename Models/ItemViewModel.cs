namespace LibrarySystem_T4.Models;

public class ItemViewModel
{
    public int      Id          { get; set; }
    public string   Title       { get; set; } = string.Empty;
    public string   Description { get; set; } = string.Empty;
    public string   Category    { get; set; } = string.Empty;
    public bool     IsAvailable { get; set; }
    public DateTime AddedDate   { get; set; } = DateTime.UtcNow;
    public string?  Author      { get; set; }
    public int?     PublishYear { get; set; }
    public int      TotalCopies { get; set; } = 1;
}