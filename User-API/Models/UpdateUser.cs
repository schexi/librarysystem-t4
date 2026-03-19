namespace Library.User.Api.Models;

public class UpdateUser
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}