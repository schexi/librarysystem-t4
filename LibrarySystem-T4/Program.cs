using LibrarySystem_T4.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrerar tjänster
builder.Services.AddControllersWithViews();

// Registrerar LoanService med HttpClient och ASP.net Core DI
// Ersätter Loans-API tidigare generella Http-Client
builder.Services.AddHttpClient<LoanService>();

// Registrerar HttpClient som anropar Categories API
builder.Services.AddHttpClient<CategoryService>(client =>
{
    client.BaseAddress = new Uri("https://kategori-cbc6adfyhwafa3fd.norwayeast-01.azurewebsites.net/");
});

// Registrerar HttpClient som anropar Items API
builder.Services.AddHttpClient<ItemService>(client =>
{
    client.BaseAddress = new Uri("https://items-api-adcac3a6hndtc0c5.norwayeast-01.azurewebsites.net/");
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

builder.Services.AddHttpClient<CategoryService>(client =>
{
    client.BaseAddress = new Uri("https://kategori-cbc6adfyhwafa3fd.norwayeast-01.azurewebsites.net/");
});

app.Run();