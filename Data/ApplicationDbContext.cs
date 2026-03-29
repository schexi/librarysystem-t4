using Microsoft.EntityFrameworkCore;
using LibrarySystem_T4.Models.Entities;

namespace LibrarySystem_T4.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity>     Users     { get; set; }
    public DbSet<LoanEntity>     Loans     { get; set; }
    public DbSet<BookingEntity>  Bookings  { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoanEntity>()
            .HasOne(l => l.User)
            .WithMany(u => u.Loans)
            .HasForeignKey(l => l.UserId);

        modelBuilder.Entity<BookingEntity>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId);
        
    }
}