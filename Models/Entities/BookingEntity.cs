namespace LibrarySystem_T4.Models.Entities;

public class BookingEntity
{
    public int      Id          { get; set; }
    public int      UserId      { get; set; }
    public string   BookTitle   { get; set; } = string.Empty;
    public string   BookAuthor  { get; set; } = string.Empty;
    public string   Isbn        { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public string   Status      { get; set; } = "active";

    public UserEntity User { get; set; } = null!;
}