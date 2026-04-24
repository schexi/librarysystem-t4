using Microsoft.EntityFrameworkCore;
using NotificationsAPI.Data;
using NotificationsAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<NotificationsDbContext>(options =>
    options.UseSqlite("Data Source=notifications.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NotificationsDbContext>();
    db.Database.Migrate();

    if (!db.Notifications.Any())
    {
        db.Notifications.AddRange(
            new Notification
            {
                UserId = 1,
                Type = "Påminnelse",
                Title = "Lämna tillbaka bok",
                Message = "Lämna tillbaka Clean Code senast imorgon.",
                ActionType = "Återlämning",
                ItemId = 5,
                Deadline = DateTime.Now.AddDays(1),
                IsRead = false,
                CreatedAt = DateTime.Now,
                ReadAt = null
            },
            new Notification
            {
                UserId = 1,
                Type = "Reservation",
                Title = "Reserverad bok tillgänglig",
                Message = "Din reserverade bok finns nu att hämta.",
                ActionType = "Hämtning",
                ItemId = 2,
                Deadline = DateTime.Now.AddDays(7),
                IsRead = false,
                CreatedAt = DateTime.Now,
                ReadAt = null
            },
            new Notification
            {
                UserId = 1,
                Type = "Försenad bok",
                Title = "Boken är försenad",
                Message = "Boken du lånat är nu försenad. Vänligen återlämna den så snart som möjligt.",
                ActionType = "Påminnelse",
                ItemId = 3,
                Deadline = DateTime.Now.AddDays(-2),
                IsRead = false,
                CreatedAt = DateTime.Now,
                ReadAt = null
            }
        );

        db.SaveChanges();
    }
}

app.UseAuthorization();
app.MapControllers();
app.Run();