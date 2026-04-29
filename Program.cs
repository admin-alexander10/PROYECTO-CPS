using Microsoft.EntityFrameworkCore;
using Proyecto_CPS.Data;
using System.ComponentModel;


var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// SESSION
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// DB CONTEXT
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLCadena"));
});

var app = builder.Build();

// ERROR HANDLING
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// STATIC FILES
app.UseStaticFiles();

// ROUTING
app.UseRouting();

// **AQUÍ ES OBLIGATORIO**
app.UseSession();

app.UseAuthorization();

// ROUTE DEFAULT
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
