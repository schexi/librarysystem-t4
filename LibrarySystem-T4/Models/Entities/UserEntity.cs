namespace LibrarySystem_T4.Models.Entities;

public class UserEntity
{
    public int      Id           { get; set; }
    public string   FirstName    { get; set; } = string.Empty;
    public string   LastName     { get; set; } = string.Empty;
    public string   Email        { get; set; } = string.Empty;
    public string   Username     { get; set; } = string.Empty;
    public string   PasswordHash { get; set; } = string.Empty;
    public string   Role         { get; set; } = "User";
    public bool     IsActive     { get; set; } = true;
    public DateTime CreatedAt    { get; set; } = DateTime.UtcNow;

    public ICollection<LoanEntity>    Loans    { get; set; } = new List<LoanEntity>();
    public ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
}