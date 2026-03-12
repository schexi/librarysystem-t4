using Microsoft.EntityFrameworkCore;
using CategoriesApi.Models;

namespace CategoriesAapi;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Representerar tabellen Categories i databasen
    public DbSet<Category> Categories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "skönlitteratur",      Description = "Påhittade berättelser om människor och liv" },
            new Category { Id = 2, Name = "Rapporter",   Description = "Interna rapporter och dokument" },
            new Category { Id = 3, Name = "historia",  Description = "Studiet av människans förflutna händelser" }
        );
    }
}