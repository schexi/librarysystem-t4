using Microsoft.EntityFrameworkCore;
using Kategori_Api.Models;

namespace Kategori_Api.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Kategori> Kategorier { get; set; }
}