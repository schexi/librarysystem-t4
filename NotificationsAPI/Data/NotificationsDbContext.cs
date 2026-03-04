using Microsoft.EntityFrameworkCore;
using NotificationsAPI.Models;

namespace NotificationsAPI.Data;

public class NotificationsDbContext : DbContext
{
    public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; } = null!;
}