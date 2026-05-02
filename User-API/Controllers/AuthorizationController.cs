using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Library.User.Api.Data;
using Library.User.Api.Models;
using Library.User.Api.Models.Entities;

namespace Library.User.Api.Controllers;

[ApiController]
[Route("api/auth")]
[Tags("Authorization")]
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
            Role         = role
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return Ok(new { message = "Konto skapat!", role });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            return Unauthorized("Fel användarnamn eller lösenord.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name,           user.Username),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role,           user.Role),
            new("FirstName",               user.FirstName)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

        return Ok(new { user.Role, user.Username, user.Id });
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/Authorization/Login");
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