namespace Library.User.Api.Models.Entities;

public class UserEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // Vi använder PasswordHash konsekvent nu
    public string Role { get; set; } = "User";
}