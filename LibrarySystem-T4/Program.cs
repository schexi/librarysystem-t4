using LibrarySystem_T4.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrerar tjänster
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); // Generell HttpClient för Loans API

// Http-client som anropar Loans-API
builder.Services.AddHttpClient<LoanService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:Loans"]!);
} );

// Registrerar HttpClient som anropar Categories API
builder.Services.AddHttpClient<CategoryService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5035/");
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();