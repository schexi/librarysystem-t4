namespace LibrarySystem_T4.Models.Entities;

public class LoanEntity
{
    public int      Id           { get; set; }
    public int      UserId       { get; set; }
    public int      ItemId       { get; set; }
    public string   BookTitle    { get; set; } = string.Empty;
    public string   BookAuthor   { get; set; } = string.Empty;
    public string   Isbn         { get; set; } = string.Empty;
    public DateTime BorrowedDate { get; set; }
    public DateTime DueDate      { get; set; }
    public bool     IsReturned   { get; set; } = false;
    public DateTime? ReturnedDate { get; set; }

    public UserEntity User { get; set; } = null!;
}