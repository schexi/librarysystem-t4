using Library.User.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Registrera databasen (SQLite)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. AKTIVERA CONTROLLERS (Detta gör att din UserController hittas)
builder.Services.AddControllers(); 

// 3. Konfigurera Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Slå på Swagger om vi kör i utvecklingsläge
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 5. KOPPLA IHOP ADRESSERNA (Gör att api/user faktiskt svarar)
app.MapControllers();

// Enkel test-rutt för att se att API:et lever
app.MapGet("/", () => "API:et rullar! Gå till /swagger för att testa dina controllers.");

app.Run();