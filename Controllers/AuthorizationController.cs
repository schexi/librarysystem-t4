using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibrarySystem_T4.Data;
using LibrarySystem_T4.Models;
using LibrarySystem_T4.Models.Entities;

namespace LibrarySystem_T4.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthorizationApiController(ApplicationDbContext context, IConfiguration configuration) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AddUserDto dto)
    {
        if (await context.Users.AnyAsync(u => u.Username == dto.Username))
            return BadRequest("Användarnamnet är upptaget.");

        var adminCode = configuration["AdminSecretCode"];
        var role = (!string.IsNullOrEmpty(dto.AdminCode) && dto.AdminCode == adminCode)
            ? "Admin" : "User";

        var user = new UserEntity
        {
            FirstName    = dto.FirstName,
            LastName     = dto.LastName,
            Email        = dto.Email,
            Username     = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash),
            Role         = role,
            CreatedAt    = DateTime.UtcNow,
            IsActive     = true
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return Ok("Användaren skapades.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            return Unauthorized("Fel användarnamn eller lösenord.");

        var secret = configuration["Jwt:Secret"] ?? "HvLibrarySecret2026SuperSecureKey123456789!";
        var key    = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds  = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name,           user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role,           user.Role),
            new Claim("FirstName",               user.FirstName)
        };

        var token = new JwtSecurityToken(
            claims:  claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            token     = tokenString,
            id        = user.Id,
            username  = user.Username,
            firstName = user.FirstName,
            lastName  = user.LastName,
            email     = user.Email,
            role      = user.Role
        });
    }
}

[Route("Authorization")]
public class AuthorizationController : Controller
{
    [HttpGet("Login")]
    public IActionResult Login() => View();

    [HttpGet("Register")]
    public IActionResult Register() => View();
}