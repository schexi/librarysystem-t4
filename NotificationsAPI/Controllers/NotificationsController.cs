using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationsAPI.Data;
using NotificationsAPI.Models;

namespace NotificationsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationsDbContext _context;

    public NotificationsController(NotificationsDbContext context)
    {
        _context = context;
    }

    // GET: api/notifications
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Notification>>> GetAll()
    {
        return await _context.Notifications
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    // GET: api/notifications/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Notification>> GetById(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);

        if (notification == null)
            return NotFound();

        return notification;
    }

    // POST: api/notifications
    [HttpPost]
    public async Task<ActionResult<Notification>> Create(Notification notification)
    {
        notification.Id = 0;
        notification.CreatedAt = DateTime.UtcNow;
        notification.IsRead = false;
        notification.ReadAt = null;

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = notification.Id }, notification);
    }

    // PUT: api/notifications/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Notification notification)
    {
        if (id != notification.Id)
            return BadRequest("Id in URL must match Id in body.");

        var exists = await _context.Notifications.AnyAsync(n => n.Id == id);
        if (!exists)
            return NotFound();

        _context.Entry(notification).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/notifications/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification == null)
            return NotFound();

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}