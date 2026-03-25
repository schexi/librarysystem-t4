namespace Library.User.Api.Models;

public class AddUserDto
{
    public string  FirstName    { get; set; } = string.Empty;
    public string  LastName     { get; set; } = string.Empty;
    public string  Email        { get; set; } = string.Empty;
    public string  Username     { get; set; } = string.Empty;
    public string  PasswordHash { get; set; } = string.Empty;
    public string? AdminCode    { get; set; }
}