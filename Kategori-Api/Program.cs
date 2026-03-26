using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore; 
using Kategori_Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var dbPath = Path.Combine(builder.Environment.ContentRootPath, "categories.db");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}


    app.MapOpenApi();
    app.MapScalarApiReference();


app.UseAuthorization();

app.MapControllers();

app.Run();