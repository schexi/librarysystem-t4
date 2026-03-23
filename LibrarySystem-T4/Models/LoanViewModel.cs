namespace LibrarySystem_T4.Models;

// Beskrivning av formulär-data för lån
public class LoanViewModel
{
    public int Id { get; set; } // Lån-ID
    public int ItemId { get; set; } // Objektets ID
    
    public string ItemTitle { get; set; } = string.Empty; // Hämtas från Items API, men sparas ej i databasen
    public string BorrowerName { get; set; } = string.Empty; // Låntagares namn (Kombinerar För o efternamn från UserAPI)
    public string BorrowerEmail { get; set; } = string.Empty; // Låntagares mail
    public DateTime BorrowedDate { get; set; } // Datum då objektet lånades
    public DateTime DueDate { get; set; } // Deadline för att återlämna objekt
    public DateTime? ReturnedDate { get; set; } // Faktiskt återlämningsdatum
}