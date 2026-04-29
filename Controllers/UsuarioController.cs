using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_CPS.Data;
using Proyecto_CPS.Models;
using Proyecto_CPS.ViewModels;

namespace Proyecto_CPS.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDBContext _context;

        public UsuarioController(AppDBContext context)
        {
            _context = context;
        }

        // ============================================
        // LISTAR TODOS LOS USUARIOS (ADMINISTRADOR)
        // ============================================
        public async Task<IActionResult> ListUsuarios()
        {
            // Verificar que sea administrador
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para acceder a esta sección";
                return RedirectToAction("Index", "Home");
            }

            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .Include(u => u.Cliente)
                .Include(u => u.Empleado)
                .OrderBy(u => u.NombreUsuario)
                .ToListAsync();

            return View(usuarios);
        }

        // ============================================
        // CREAR NUEVO USUARIO (GET)
        // ============================================
        public IActionResult Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción";
                return RedirectToAction("ListUsuarios");
            }

            // Cargar roles para el dropdown (solo Mesero y Cocinero)
            ViewBag.Roles = new SelectList(
                _context.Roles.Where(r => r.NombreRol == "Mesero" || r.NombreRol == "Cocinero"),
                "IdRol",
                "NombreRol"
            );

            return View(new UsuarioVM());
        }

        // ============================================
        // CREAR NUEVO USUARIO (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioVM usuarioVM)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción";
                return RedirectToAction("ListUsuarios");
            }

            // Validar que las contraseñas coincidan
            if (usuarioVM.ContrasenaVM != usuarioVM.ConfirmarContrasenaVM)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                ViewBag.Roles = new SelectList(
                    _context.Roles.Where(r => r.NombreRol == "Mesero" || r.NombreRol == "Cocinero"),
                    "IdRol",
                    "NombreRol"
                );
                return View(usuarioVM);
            }

            // Verificar si el correo ya existe
            var usuarioExistente = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == usuarioVM.CorreoVM);

            if (usuarioExistente != null)
            {
                ViewBag.Error = "El correo ya está registrado";
                ViewBag.Roles = new SelectList(
                    _context.Roles.Where(r => r.NombreRol == "Mesero" || r.NombreRol == "Cocinero"),
                    "IdRol",
                    "NombreRol"
                );
                return View(usuarioVM);
            }

            try
            {
                // Crear nuevo usuario (Mesero o Cocinero)
                var usuario = new Usuario
                {
                    NombreUsuario = usuarioVM.NombreUsuarioVM,
                    Correo = usuarioVM.CorreoVM,
                    Contrasena = usuarioVM.ContrasenaVM,
                    RolId = usuarioVM.RolIdVM,
                    Estado = true,
                    FechaCreacion = DateTime.Now,
                    EmpleadoId = null,
                    ClienteId = null
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Usuario creado exitosamente";
                return RedirectToAction("ListUsuarios");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al crear usuario: " + ex.Message;
                ViewBag.Roles = new SelectList(
                    _context.Roles.Where(r => r.NombreRol == "Mesero" || r.NombreRol == "Cocinero"),
                    "IdRol",
                    "NombreRol"
                );
                return View(usuarioVM);
            }
        }

        // ============================================
        // EDITAR USUARIO (GET)
        // ============================================
        public async Task<IActionResult> Edit(int? id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción";
                return RedirectToAction("ListUsuarios");
            }

            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            // Convertir a ViewModel
            var usuarioVM = new UsuarioVM
            {
                IdUsuarioVM = usuario.IdUsuario,
                NombreUsuarioVM = usuario.NombreUsuario,
                CorreoVM = usuario.Correo,
                ContrasenaVM = usuario.Contrasena,
                ConfirmarContrasenaVM = usuario.Contrasena,
                EstadoVM = usuario.Estado,
                RolIdVM = usuario.RolId
            };

            ViewBag.Roles = new SelectList(
                _context.Roles.Where(r => r.NombreRol == "Mesero" || r.NombreRol == "Cocinero"),
                "IdRol",
                "NombreRol",
                usuario.RolId
            );

            return View(usuarioVM);
        }

        // ============================================
        // EDITAR USUARIO (POST)
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UsuarioVM usuarioVM)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción";
                return RedirectToAction("ListUsuarios");
            }

            if (id != usuarioVM.IdUsuarioVM)
            {
                return NotFound();
            }

            // Validar que las contraseñas coincidan
            if (usuarioVM.ContrasenaVM != usuarioVM.ConfirmarContrasenaVM)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                ViewBag.Roles = new SelectList(
                    _context.Roles.Where(r => r.NombreRol == "Mesero" || r.NombreRol == "Cocinero"),
                    "IdRol",
                    "NombreRol"
                );
                return View(usuarioVM);
            }

            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                // Actualizar datos
                usuario.NombreUsuario = usuarioVM.NombreUsuarioVM;
                usuario.Correo = usuarioVM.CorreoVM;
                usuario.Contrasena = usuarioVM.ContrasenaVM;
                usuario.RolId = usuarioVM.RolIdVM;
                usuario.Estado = usuarioVM.EstadoVM;

                _context.Update(usuario);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Usuario actualizado exitosamente";
                return RedirectToAction("ListUsuarios");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuarioVM.IdUsuarioVM))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al actualizar: " + ex.Message;
                ViewBag.Roles = new SelectList(
                    _context.Roles.Where(r => r.NombreRol == "Mesero" || r.NombreRol == "Cocinero"),
                    "IdRol",
                    "NombreRol"
                );
                return View(usuarioVM);
            }
        }

        // ============================================
        // ELIMINAR USUARIO (GET)
        // ============================================
        public async Task<IActionResult> Delete(int? id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción";
                return RedirectToAction("ListUsuarios");
            }

            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // ============================================
        // ELIMINAR USUARIO (POST)
        // ============================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción";
                return RedirectToAction("ListUsuarios");
            }

            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario != null)
                {
                    _context.Usuarios.Remove(usuario);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Usuario eliminado exitosamente";
                }

                return RedirectToAction("ListUsuarios");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar: " + ex.Message;
                return RedirectToAction("ListUsuarios");
            }
        }

        // ============================================
        // VER PERFIL DEL USUARIO LOGUEADO
        // ============================================
        public IActionResult Perfil()
        {
            var userId = HttpContext.Session.GetInt32("IdUsuario");
            if (userId == null)
            {
                return RedirectToAction("Login", "Acceso");
            }

            var usuario = _context.Usuarios
                .Include(u => u.Cliente)
                .Include(u => u.Empleado)
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.IdUsuario == userId.Value);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // ============================================
        // EDITAR PERFIL PROPIO (GET)
        // ============================================
        [HttpGet]
        public IActionResult EditPerfil()
        {
            int? userId = HttpContext.Session.GetInt32("IdUsuario");
            if (userId == null)
                return RedirectToAction("Login", "Acceso");

            var usuario = _context.Usuarios.Find(userId.Value);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // ============================================
        // EDITAR PERFIL PROPIO (POST)
        // ============================================
        [HttpPost]
        public IActionResult EditPerfil(Usuario usuario)
        {
            int? userId = HttpContext.Session.GetInt32("IdUsuario");
            if (userId == null)
                return RedirectToAction("Login", "Acceso");

            if (!ModelState.IsValid)
                return View(usuario);

            var usuarioExistente = _context.Usuarios.Find(userId.Value);
            if (usuarioExistente == null)
                return NotFound();

            usuarioExistente.NombreUsuario = usuario.NombreUsuario;
            usuarioExistente.Correo = usuario.Correo;
            usuarioExistente.Contrasena = usuario.Contrasena;

            _context.SaveChanges();

            return RedirectToAction("Perfil");
        }

        // ============================================
        // HELPER: VERIFICAR SI USUARIO EXISTE
        // ============================================
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }
}
