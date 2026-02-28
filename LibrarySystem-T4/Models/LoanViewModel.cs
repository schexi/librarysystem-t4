namespace LibrarySystem_T4.Models;

// Beskrivning av formulär-data för lån
public class LoanViewModel
{
    public int LoanId { get; set; }
    public int ItemId { get; set; }
    public string BorrowerName { get; set; } = string.Empty;
    public string BorrowerEmail { get; set; } = string.Empty;
    public DateTime BorrowedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }  // Får vara tom, eftersom återlämning sker senare  
}