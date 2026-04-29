using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_CPS.Data;
using Proyecto_CPS.Models;
using Proyecto_CPS.ViewModels;

namespace Proyecto_CPS.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AppDBContext _context;

        public AccesoController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Acceso/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u =>
                    u.Correo == model.CorreoVM &&
                    u.Contrasena == model.ContrasenaVM &&
                    u.Estado
                );

            if (usuario != null)
            {
                HttpContext.Session.SetString("Email", usuario.Correo);
                HttpContext.Session.SetString("Nombre", usuario.NombreUsuario);
                HttpContext.Session.SetString("Rol", usuario.Rol.NombreRol);
                HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);

                // REDIRECCIÓN SEGÚN ROL
                switch (usuario.Rol.NombreRol)
                {
                    case "Administrador":
                        return RedirectToAction("PanelAdmin", "Admin");

                    case "Mesero":
                        return RedirectToAction("PanelMesero", "Mesero");

                    case "Cocinero":
                        return RedirectToAction("PanelCocinero", "Cocinero");

                    case "Cliente":
                        return RedirectToAction("Index", "Home");

                    default:
                        return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Credenciales inválidas o usuario inactivo";
            return View(model);
        }

        // GET: Acceso/Registrar
        public IActionResult Registrar()
        {
            return View(new UsuarioVM());
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioVM usuarioVM)
        {
            // Verificar si las contraseñas coinciden
            if (usuarioVM.ContrasenaVM != usuarioVM.ConfirmarContrasenaVM)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View(usuarioVM);
            }

            // Verificar si el correo ya existe
            var usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == usuarioVM.CorreoVM);

            if (usuarioExistente != null)
            {
                ViewBag.Error = "El correo ya está registrado";
                return View(usuarioVM);
            }

            // Crear nuevo usuario como Cliente
            var usuario = new Usuario
            {
                NombreUsuario = usuarioVM.NombreUsuarioVM,
                Correo = usuarioVM.CorreoVM,
                Contrasena = usuarioVM.ContrasenaVM,
                RolId = 4, // Rol Cliente
                Estado = true,
                FechaCreacion = DateTime.Now
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Auto-login después del registro
            HttpContext.Session.SetString("Email", usuario.Correo);
            HttpContext.Session.SetString("Nombre", usuario.NombreUsuario);
            HttpContext.Session.SetString("Rol", "Cliente");
            HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);

            return RedirectToAction("Index", "Home");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
