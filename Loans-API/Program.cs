using Loans_API.KeyFilters;
using LoansApi.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5100"); // Port för Loans API

// Registrerar tjänster
builder.Services.AddControllers();
builder.Services.AddOpenApi(); // OpenAPI/Scalar för API-dokumentation
builder.Services.AddDbContext<LoansContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // Hämtar connection string från appsettings.json
builder.Services.AddScoped<ApiKeyFilter>(); // Registrerar API-nyckelfilter

var app = builder.Build();

// Kör migrationer automatiskt vid startup - skapar databasen om den inte finns
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LoansContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Scalar UI tillgänglig på /scalar/v1
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
