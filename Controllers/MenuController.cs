using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_CPS.Data;
using Proyecto_CPS.Models;
using Microsoft.AspNetCore.Http;

namespace Proyecto_CPS.Controllers
{
    public class MenuController : Controller
    {
        private readonly AppDBContext _context;

        public MenuController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Menu (Público)
        public async Task<IActionResult> Index()
        {
            try
            {
                var menus = await _context.Menus
                    .Include(m => m.CategoriaMenus)
                    .Where(m => m.Precio > 0)
                    .OrderBy(m => m.CategoriaMenuId)
                    .ThenBy(m => m.Nombre)
                    .ToListAsync();

                var categorias = await _context.CategoriaMenus
                    .OrderBy(c => c.NombreDeCategoria)
                    .ToListAsync();

                ViewBag.Categorias = categorias;
                return View(menus);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar el menú: " + ex.Message;
                return View(new List<Menu>());
            }
        }

        // GET: Menu/Admin (Panel de administración)
        public async Task<IActionResult> Admin()
        {
            // Verificar si el usuario es administrador
            if (HttpContext.Session.GetString("Rol") != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para acceder a esta sección";
                return RedirectToAction("Index", "Home");
            }

            var menus = await _context.Menus
                .Include(m => m.CategoriaMenus)
                .OrderBy(m => m.CategoriaMenuId)
                .ThenBy(m => m.Nombre)
                .ToListAsync();

            return View(menus);
        }

        // GET: Menu/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var menu = await _context.Menus
                .Include(m => m.CategoriaMenus)
                .FirstOrDefaultAsync(m => m.IdMenu == id);

            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menu/Create
        public async Task<IActionResult> Create()
        {
            // Verificar permisos
            if (HttpContext.Session.GetString("Rol") != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción";
                return RedirectToAction("Admin");
            }

            ViewBag.Categorias = new SelectList(await _context.CategoriaMenus.ToListAsync(), "IdCategoria", "NombreDeCategoria");
            return View();
        }

        // POST: Menu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Menu menu)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción";
                return RedirectToAction("Admin");
            }

            ModelState.Remove("CategoriaMenus");
            ModelState.Remove("Comentarios");
            ModelState.Remove("DetalleMenuPedidos");

            if (ModelState.IsValid)
            {
                menu.CategoriaMenus = null;
                menu.DetalleMenuPedidos = null;
                menu.Comentarios = null;

                _context.Add(menu);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Platillo creado exitosamente!";
                return RedirectToAction(nameof(Admin));
            }

            ViewBag.Categorias = new SelectList(
                await _context.CategoriaMenus.ToListAsync(),
                "IdCategoria",
                "NombreDeCategoria",
                menu.CategoriaMenuId
            );
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Error en el formulario: " +
                    string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
            }
            return View(menu);
        }


        // GET: Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción";
                return RedirectToAction("Admin");
            }

            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            ViewBag.Categorias = new SelectList(await _context.CategoriaMenus.ToListAsync(), "IdCategoria", "NombreDeCategoria", menu.CategoriaMenuId);
            return View(menu);
        }

        // POST: Menu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Menu menu)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción";
                return RedirectToAction("Admin");
            }

            if (id != menu.IdMenu)
                return NotFound();

            // EXCLUIR VALIDACIÓN QUE BLOQUEA EL GUARDADO
            ModelState.Remove("CategoriaMenus");
            ModelState.Remove("DetalleMenuPedidos");
            ModelState.Remove("Comentarios");

            if (ModelState.IsValid)
            {
                var menuDB = await _context.Menus.FindAsync(id);
                if (menuDB == null)
                    return NotFound();

                menuDB.Nombre = menu.Nombre;
                menuDB.Precio = menu.Precio;
                menuDB.Descripcion = menu.Descripcion;
                menuDB.CategoriaMenuId = menu.CategoriaMenuId;

                await _context.SaveChangesAsync();

                TempData["Success"] = "Platillo actualizado exitosamente!";
                return RedirectToAction(nameof(Admin));
            }

            ViewBag.Categorias = new SelectList(
                await _context.CategoriaMenus.ToListAsync(),
                "IdCategoria", "NombreDeCategoria", menu.CategoriaMenuId);

            return View(menu);
        }



        // GET: Menu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción";
                return RedirectToAction("Admin");
            }

            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.CategoriaMenus)
                .FirstOrDefaultAsync(m => m.IdMenu == id);

            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción";
                return RedirectToAction("Admin");
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Platillo eliminado exitosamente!";
            }
            return RedirectToAction(nameof(Admin));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.IdMenu == id);
        }
    }
}
