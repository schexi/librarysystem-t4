using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LibrarySystem_T4.Data;
using LibrarySystem_T4.Models;
using LibrarySystem_T4.Models.Entities;

namespace LibrarySystem_T4.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userId      = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user        = await context.Users.FindAsync(userId);
        var activeLoans = await context.Loans.CountAsync(l => l.UserId == userId && !l.IsReturned);
        return user == null ? NotFound() : Ok(new { user, activeLoans });
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
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound();
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return Ok();
    }
}

// MVC-vyer — INGEN [Authorize] här, auth sköts av JavaScript
[Route("User")]
public class UserPageController : Controller
{
    [HttpGet("Profile")]
    public IActionResult Profile() => View("~/Views/User/Profile.cshtml");

    [HttpGet("Update")]
    public IActionResult Update() => View("~/Views/User/Update.cshtml");

    [HttpGet("Loans")]
    public IActionResult Loans() => View("~/Views/User/Loans.cshtml");
}