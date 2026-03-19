using Microsoft.EntityFrameworkCore;
using LibrarysystemLoans.Models;

namespace LibrarysystemLoans.Data;

// Databaskontexten som hanterar kommunikationen med SQLite-databasen
public class LoansContext : DbContext
{
    public LoansContext(DbContextOptions<LoansContext> options) : base(options) { }

    public DbSet<Loan> Loans { get; set; } // Tabell för lån
    public DbSet<LoanStatus> LoanStatuses { get; set; } // Tabell för lånestatus

    // Konfigurerar databasen och seedar grundläggande statusar
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // FK-koppling mellan Loan och LoanStatus
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.LoanStatus)
            .WithMany(s => s.Loans)
            .HasForeignKey(l => l.LoanStatusId);

        // Seed-data - grundläggande statusar skapas automatiskt
        modelBuilder.Entity<LoanStatus>().HasData(
            new LoanStatus { Id = 1, Name = "Aktiv" },
            new LoanStatus { Id = 2, Name = "Försenad" },
            new LoanStatus { Id = 3, Name = "Återlämnad" }
        );
    }
}