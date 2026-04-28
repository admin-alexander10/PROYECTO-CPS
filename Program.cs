using Microsoft.EntityFrameworkCore;
using Proyecto_CPS.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar Servicios
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLCadena"));
});

// INDISPENSABLE: Agregar servicio de sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// INDISPENSABLE: Habilitar sesiones antes de Authorization
app.UseSession();

app.UseAuthorization();

// CONFIGURACIÓN DE RUTA: 
// Cambié "Acceso/Login" por "Menu/Index" para que entres directo al menú al dar Play
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Menu}/{action=Index}/{id?}");

app.Run();