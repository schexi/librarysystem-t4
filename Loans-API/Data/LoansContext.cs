using Microsoft.EntityFrameworkCore;
using LoansApi.Models;

namespace LoansApi.Data;

// Databaskontexten som hanterar kommunikationen med SQLite-databasen
public class LoansContext : DbContext
{
    public LoansContext(DbContextOptions<LoansContext> options) : base(options) { }

    public DbSet<Loan> Loans { get; set; } // Tabell för lån
}