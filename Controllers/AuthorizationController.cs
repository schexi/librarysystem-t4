using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Library.User.Api.Models;
using Library.User.Api.Data;
using Library.User.Api.Models.Entities;
using Library.User.Api.Infrastructure;
using System.Security.Claims;

namespace Library.User.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthorizationController(ApplicationDbContext context, TokenProvider tokenProvider) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = context.Users.FirstOrDefault(u => u.Email == request.Email);

        if (user == null || user.PasswordHash != request.Password)
        {
            return Unauthorized("Fel email eller lösenord.");
        }

        var token = tokenProvider.Create(user);
        return Ok(new { token });
    }

    [Authorize] // Alla inloggade kommer åt denna
    [HttpGet("profil")]
    public IActionResult GetProfile()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        return Ok(new { message = "Välkommen till din profil!", email });
    }

    [Authorize(Roles = "Admin")] // ENDAST Admins kommer åt denna
    [HttpGet("admin-data")]
    public IActionResult GetAdminData()
    {
        return Ok(new { message = "Här är hemlig data som bara Admins kan se!" });
    }
}