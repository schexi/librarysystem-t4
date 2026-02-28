namespace LoansApi.Models;

public class Loan
{
    public int Id { get; set; } // Lån-ID
    public int ItemId { get; set; } // Objektets ID
    public string BorrowerName { get; set; } = string.Empty; // Initiera som en tom string för att minimera errors
    public string BorrowerEmail { get; set; } = string.Empty; // Låntagares mail, och ovanför: låntagares namn
    public DateTime BorrowedDate { get; set; } // Datum då objektet lånades
    public DateTime DueDate { get; set; }  // Deadline för att återlämna objekt
    public DateTime? ReturnedDate { get; set; }  // Faktiskt återlämningsdatum
}