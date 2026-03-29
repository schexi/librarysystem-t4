using LibrarySystem_T4.Data;
using LibrarySystem_T4.Models.Entities;

namespace LibrarySystem_T4.Seed;

public static class AdminSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        if (!db.Users.Any(u => u.Role == "admin"))
        {
            db.Users.Add(new UserEntity
            {
                FirstName    = "admin",
                LastName     = "HV",
                Email        = "admin@hvbibliotek.se",
                Username     = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role         = "Admin",
                IsActive     = true,
                CreatedAt    = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

        if (!db.Users.Any(u => u.Username == "anna"))
        {
            var anna = new UserEntity
            {
                FirstName    = "Anna",
                LastName     = "Lindqvist",
                Email        = "anna@hv.se",
                Username     = "anna",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Anna123!"),
                Role         = "User",
                IsActive     = true,
                CreatedAt    = DateTime.UtcNow.AddMonths(-2)
            };
            db.Users.Add(anna);
            await db.SaveChangesAsync();

            db.Loans.AddRange(
                new LoanEntity
                {
                    UserId       = anna.Id,
                    BookTitle    = "Millennium: Män som hatar kvinnor",
                    BookAuthor   = "Stieg Larsson",
                    Isbn         = "978-91-7263-498-4",
                    BorrowedDate = DateTime.UtcNow.AddDays(-10),
                    DueDate      = DateTime.UtcNow.AddDays(11),
                    IsReturned   = false
                },
                new LoanEntity
                {
                    UserId       = anna.Id,
                    BookTitle    = "Sapiens",
                    BookAuthor   = "Yuval Noah Harari",
                    Isbn         = "978-91-89062-00-5",
                    BorrowedDate = DateTime.UtcNow.AddDays(-30),
                    DueDate      = DateTime.UtcNow.AddDays(-9),
                    IsReturned   = true,
                    ReturnedDate = DateTime.UtcNow.AddDays(-10)
                }
            );
            db.Bookings.Add(new BookingEntity
            {
                UserId      = anna.Id,
                BookTitle   = "Röda rummet",
                BookAuthor  = "August Strindberg",
                Isbn        = "978-91-7324-123-0",
                BookingDate = DateTime.UtcNow.AddDays(-3),
                Status      = "active"
            });
            await db.SaveChangesAsync();
        }
    }
}