namespace LibrarysystemLoans.Models;

// Representerar möjliga statusar för ett lån
public class LoanStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Aktiv, Försenad, Återlämnad

    // Navigationsegenskap - ett status kan tillhöra många lån (en-till-många)
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}