using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Library.User.Api.Data;
using Library.User.Api.Seed;

var builder = WebApplication.CreateBuilder(args);

// Lägg till MVC
builder.Services.AddControllersWithViews();

// Swagger (för API-dokumentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQLite-databas
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=library.db"));

// Cookie-baserad autentisering
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authorization/Login";
        options.LogoutPath = "/Authorization/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

// CORS för frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
        policy.WithOrigins("https://localhost:3000") // ändra till din frontend-port
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

var app = builder.Build();

// Seed databasen
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
    await AdminSeeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

app.UseCors("FrontendPolicy"); // Viktigt!
app.UseAuthentication();        // Kör autentisering
app.UseAuthorization();         // Kör auktorisering

// Default route
app.MapGet("/", context => {
    context.Response.Redirect("/Authorization/Login");
    return Task.CompletedTask;
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authorization}/{action=Login}/{id?}");

// API controllers
app.MapControllers();

app.Run();