namespace Library.User.Api.Models.Entities;

public class UserEntity
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}