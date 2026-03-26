using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Library.User.Api.Data;

namespace Library.User.Api.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize]
public class BookingsController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetUserBookings(int userId)
    {
        var requesterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin     = User.IsInRole("Admin");

        if (requesterId != userId && !isAdmin)
            return Forbid();

        var bookings = await context.Bookings
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();

        return Ok(bookings);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetMyActiveBookings()
    {
        var userId   = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var bookings = await context.Bookings
            .Where(b => b.UserId == userId && b.Status == "active")
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();

        return Ok(bookings);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var userId  = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var booking = await context.Bookings.FindAsync(id);

        if (booking == null) return NotFound();
        if (booking.UserId != userId && !User.IsInRole("Admin")) return Forbid();

        context.Bookings.Remove(booking);
        await context.SaveChangesAsync();
        return Ok(new { message = "Bokning avbokad." });
    }
}