namespace LoansApi.Models;

// Representerar ett lån i systemet
public class Loan
{
    public int Id { get; set; }
    public int ItemId { get; set; } // ID för lånat objekt, kopplas till Items API vid driftsättning
    public string BorrowerName { get; set; } = string.Empty; // Låntagarens namn, kopplas till Users API vid driftsättning
    public string BorrowerEmail { get; set; } = string.Empty; // Låntagarens email
    public DateTime BorrowedDate { get; set; } // Sätts automatiskt vid skapande
    public DateTime DueDate { get; set; } // Förfallodatum
    public DateTime? ReturnedDate { get; set; } // Null tills lånet återlämnas
}
