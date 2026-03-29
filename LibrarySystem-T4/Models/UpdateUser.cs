namespace LibrarySystem_T4.Models;

public class UpdateUserDto
{
    public string  FirstName    { get; set; } = string.Empty;
    public string  LastName     { get; set; } = string.Empty;
    public string  Email        { get; set; } = string.Empty;
    public string? PasswordHash { get; set; }
}