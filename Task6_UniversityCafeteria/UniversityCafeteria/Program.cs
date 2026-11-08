using Microsoft.EntityFrameworkCore;
using UniversityCafeteria.Data;

var builder = WebApplication.CreateBuilder(args);

// 1) Add MVC.
builder.Services.AddControllersWithViews();

// 2) Connect Entity Framework Core to SQL Server.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// 3) Code First: create the database and tables automatically on first run.
// For this small task, EnsureCreated() is simpler than migrations.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// 4) Open the Category list when the application starts.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Category}/{action=Index}/{id?}");

app.Run();
