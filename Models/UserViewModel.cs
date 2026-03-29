namespace LibrarySystem_T4.Models;

// Representerar när fält hämtas från User API till Loans
public class UserViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}