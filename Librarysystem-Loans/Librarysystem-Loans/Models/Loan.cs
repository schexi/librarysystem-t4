namespace LibrarysystemLoans.Models;

// Representerar ett lån i systemet
public class Loan
{
    public int Id { get; set; }
    public int ItemId { get; set; } // ID för lånat objekt, kopplas till Items API senare
    public string BorrowerName { get; set; } = string.Empty; // Låntagarens namn, kopplas till Users API senare
    public string BorrowerEmail { get; set; } = string.Empty; // Låntagarens email
    public DateTime BorrowedDate { get; set; } // Sätts automatiskt vid skapande
    public DateTime DueDate { get; set; } // Förfallodatum, 4 veckor framåt som standard
    public DateTime? ReturnedDate { get; set; } // Null tills lånet återlämnas

    // FK-koppling till LoanStatus (en-till-många)
    public int LoanStatusId { get; set; }
    public LoanStatus LoanStatus { get; set; } = null!; // Navigationsegenskap
}