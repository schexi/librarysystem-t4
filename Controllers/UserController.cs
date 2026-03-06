using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.User.Api.Data;
using Library.User.Api.Models;
using Library.User.Api.Models.Entities;

namespace Library.User.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound("Användaren hittades inte.");
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(AddUserDto dto)
    {
        var user = new UserEntity
        {
            Name = dto.Name,
            LastName = dto.LastName,
            Email = dto.Email,
            EmployeeId = dto.EmployeeId,
            PasswordHash = dto.PasswordHash,
            Role = "User" // Standardroll
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUser dto)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound("Användaren finns inte.");

        user.Name = dto.Name;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.EmployeeId = dto.EmployeeId;
        user.PasswordHash = dto.PasswordHash;
        // Här kan du även lägga till user.Role = dto.Role om du vill kunna ändra roll via PUT

        await context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPatch("{id}/make-admin")]
    public async Task<IActionResult> MakeAdmin(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound("Användaren hittades inte.");

        user.Role = "Admin"; 
        await context.SaveChangesAsync();

        return Ok($"{user.Name} är nu Admin!");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound();

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return Ok("Användare raderad.");
    }
}