using Microsoft.EntityFrameworkCore;
using Library.User.Api.Models.Entities;

namespace Library.User.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Models.Entities.UserEntity> Users { get; set; }
}