namespace Items_API.Data;

using Microsoft.EntityFrameworkCore;
using Items_API.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Item> Items { get; set; }
}