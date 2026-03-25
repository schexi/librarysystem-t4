using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Library.User.Api.Data;
using Library.User.Api.Models;
using Library.User.Api.Models.Entities;

namespace Library.User.Api.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await context.Users.FindAsync(userId);
        var activeLoans = await context.Loans.CountAsync(l => l.UserId == userId && !l.IsReturned);
        return user == null ? NotFound() : Ok(new { user, activeLoans });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await context.Users.FindAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AddUser([FromBody] AddUserDto dto)
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
            Role         = "User"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user   = await context.Users.FindAsync(userId);
        if (user == null) return NotFound();

        user.FirstName = dto.FirstName;
        user.LastName  = dto.LastName;
        user.Email     = dto.Email;

        if (!string.IsNullOrEmpty(dto.PasswordHash))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

        await context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound();
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return Ok();
    }
}

[Route("User")]
[Authorize]
public class UserViewController : Controller
{
    [HttpGet("Profile")]
    public IActionResult Profile() => View();

    [HttpGet("Update")]
    public IActionResult Update() => View();
}