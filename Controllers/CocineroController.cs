using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_CPS.Data;
using Proyecto_CPS.Models;

namespace Proyecto_CPS.Controllers
{
    public class CocineroController : Controller
    {
        private readonly AppDBContext _context;

        public CocineroController(AppDBContext context)
        {
            _context = context;
        }

        // --- PANEL PRINCIPAL ---
        public IActionResult PanelCocinero()
        {
            return View();
        }

        // ==========================================================
        // ÓRDENES PENDIENTES (Recién enviadas por el mesero)
        // ==========================================================
        public async Task<IActionResult> EnProceso()
        {
            var ordenes = await _context.Ordenes
                .Include(o => o.Pedido)
                    .ThenInclude(p => p.Usuario)
                .Include(o => o.Pedido)
                    .ThenInclude(p => p.DetalleMenuPedidos)
                        .ThenInclude(d => d.Menu)
                .Where(o => o.EstadoCocina == "Pendiente")
                .OrderBy(o => o.Fecha)
                .ToListAsync();

            return View(ordenes);
        }

        // ==========================================================
        // ACEPTAR ORDEN (Pendiente -> EnProceso)
        // ==========================================================
        [HttpPost]
        public async Task<IActionResult> AceptarOrden(int id)
        {
            try
            {
                var orden = await _context.Ordenes.FindAsync(id);

                if (orden == null)
                    return Json(new { success = false, message = "Orden no encontrada" });

                if (orden.EstadoCocina != "Pendiente")
                    return Json(new { success = false, message = "Esta orden ya fue aceptada" });

                orden.EstadoCocina = "EnProceso";
                _context.Ordenes.Update(orden);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Orden aceptada y en proceso de preparación" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // ==========================================================
        // ÓRDENES EN COCINA (En preparación)
        // ==========================================================
        public IActionResult Cocina()
        {
            var ordenes = _context.Ordenes
                .Include(o => o.Pedido)
                    .ThenInclude(p => p.Usuario)
                .Include(o => o.Pedido)
                    .ThenInclude(p => p.DetalleMenuPedidos)
                        .ThenInclude(d => d.Menu)
                            .ThenInclude(m => m.CategoriaMenus)
                .Where(o => o.EstadoCocina == "EnProceso")
                .OrderBy(o => o.Fecha)
                .ToList();

            return View(ordenes);
        }

        // ==========================================================
        // MARCAR COMO TERMINADO (EnProceso -> Terminado)
        // ==========================================================
        [HttpPost]
        public async Task<IActionResult> MarcarTerminado(int id)
        {
            try
            {
                var orden = await _context.Ordenes.FindAsync(id);

                if (orden == null)
                    return Json(new { success = false, message = "Orden no encontrada" });

                if (orden.EstadoCocina != "EnProceso")
                    return Json(new { success = false, message = "Esta orden no está en proceso" });

                orden.EstadoCocina = "Terminado";
                _context.Ordenes.Update(orden);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Orden marcada como terminada" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // ==========================================================
        // ÓRDENES TERMINADAS (Listas para enviar al mesero)
        // ==========================================================
        public async Task<IActionResult> Terminadas()
        {
            var ordenes = await _context.Ordenes
                .Include(o => o.Pedido)
                    .ThenInclude(p => p.Usuario)
                .Include(o => o.Pedido)
                    .ThenInclude(p => p.DetalleMenuPedidos)
                        .ThenInclude(d => d.Menu)
                .Where(o => o.EstadoCocina == "Terminado")
                .OrderByDescending(o => o.Fecha)
                .ToListAsync();

            return View(ordenes);
        }

        // ==========================================================
        // ENVIAR AL MESERO (Terminado -> Entregado)
        // ==========================================================
        [HttpPost]
        public async Task<IActionResult> EnviarAMesero(int id)
        {
            try
            {
                var orden = await _context.Ordenes.FindAsync(id);

                if (orden == null)
                    return Json(new { success = false, message = "Orden no encontrada" });

                if (orden.EstadoCocina != "Terminado")
                    return Json(new { success = false, message = "Esta orden no está terminada" });

                orden.EstadoCocina = "Entregado";
                _context.Ordenes.Update(orden);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Orden enviada al mesero para entrega" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}
