using Microsoft.EntityFrameworkCore;
using LoansApi.Models;

namespace LoansApi.Data;

public class LoansContext : DbContext
{
    public LoansContext(DbContextOptions<LoansContext> options) : base(options) { }
    public DbSet<Loan> Loans { get; set; }
}