using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using LibrarySystem_T4.Data;
using LibrarySystem_T4.Models;
using LibrarySystem_T4.Models.Entities;

namespace LibrarySystem_T4.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
[Tags("Admin")]
public class AdminController(ApplicationDbContext context, ILogger<AdminController> logger) : ControllerBase
{
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var total   = await context.Users.CountAsync();
        var admins  = await context.Users.CountAsync(u => u.Role == "Admin");
        var regular = await context.Users.CountAsync(u => u.Role == "User");
        var loans   = await context.Loans.CountAsync(l => !l.IsReturned);
        return Ok(new { total, admins, regular, loans });
    }

    [HttpGet("user-list")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody] AddUserDto dto)
    {
        if (await context.Users.AnyAsync(u => u.Username == dto.Username))
            return BadRequest("Användarnamnet är redan taget.");

        var user = new UserEntity
        {
            FirstName    = dto.FirstName,
            LastName     = dto.LastName,
            Email        = dto.Email,
            Username     = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash),
            Role         = string.IsNullOrEmpty(dto.AdminCode) ? "User" : "Admin"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPatch("user/{id}/change-role")]
    public async Task<IActionResult> ChangeRole(int id, [FromQuery] string newRole)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound("Användaren hittades inte.");
        user.Role = newRole;
        await context.SaveChangesAsync();
        logger.LogWarning("Admin ändrade roll för användare {Id} till {Role}", id, newRole);
        return Ok(new { message = $"Användarens roll är nu {newRole}" });
    }

    [HttpPut("user/{id}/force-update")]
    public async Task<IActionResult> ForceUpdate(int id, [FromBody] UserEntity updatedData)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound();
        user.FirstName = updatedData.FirstName;
        user.LastName  = updatedData.LastName;
        user.Email     = updatedData.Email;
        user.Username  = updatedData.Username;
        user.Role      = updatedData.Role;
        await context.SaveChangesAsync();
        return Ok(new { message = "Användardatan har skrivits över av Admin." });
    }

    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound();
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return Ok(new { message = "Användaren har raderats." });
    }
}

// MVC-vyer — auth sköts av JavaScript
[Route("Admin")]
public class AdminViewController : Controller
{
    [HttpGet("")]
    public IActionResult Index() => View("~/Views/Admin/Admin.cshtml");

    [HttpGet("Users")]
    public IActionResult Users() => View("~/Views/Admin/Users.cshtml");

    [HttpGet("Loans")]
    public IActionResult Loans() => View("~/Views/Admin/Loans.cshtml");
}