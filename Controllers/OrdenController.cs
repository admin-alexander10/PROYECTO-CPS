using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_CPS.Data;
using Proyecto_CPS.Models;

namespace Proyecto_CPS.Controllers
{
    public class OrdenController : Controller
    {
        private readonly AppDBContext _context;

        public OrdenController(AppDBContext context)
        {
            _context = context;
        }

        // ==========================================================
        // CREAR ORDEN DESDE PEDIDO (Mesero marca "En Proceso")
        // ==========================================================
        [HttpPost]
        public IActionResult CrearDesdePedido(int idPedido)
        {
            try
            {
                // Buscar el pedido
                var pedido = _context.Pedidos
                    .Include(p => p.DetalleMenuPedidos)
                    .ThenInclude(d => d.Menu)
                    .FirstOrDefault(p => p.IdPedido == idPedido);

                if (pedido == null)
                    return Json(new { success = false, message = "El pedido no existe." });

                // Validar que no tenga una orden creada
                var existeOrden = _context.Ordenes.FirstOrDefault(o => o.PedidoId == idPedido);
                if (existeOrden != null)
                    return Json(new { success = false, message = "La orden ya fue generada." });

                // Crear orden con estado "Pendiente"
                var orden = new Orden
                {
                    PedidoId = idPedido,
                    EstadoCocina = "Pendiente",
                    Fecha = DateTime.Now
                };

                _context.Ordenes.Add(orden);
                _context.SaveChanges();

                return Json(new { success = true, message = "Pedido enviado a cocina." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // ==========================================================
        // ÓRDENES LISTAS PARA ENTREGAR (Para el mesero)
        // ==========================================================
        public async Task<IActionResult> ListasParaEntregar()
        {
            // Verificar que sea mesero
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Mesero")
            {
                TempData["Error"] = "No tienes permisos para acceder a esta sección";
                return RedirectToAction("Index", "Home");
            }

            var ordenes = await _context.Ordenes
                .Include(o => o.Pedido)
                    .ThenInclude(p => p.Usuario)
                .Include(o => o.Pedido)
                    .ThenInclude(p => p.DetalleMenuPedidos)
                        .ThenInclude(d => d.Menu)
                .Where(o => o.EstadoCocina == "Entregado")
                .OrderBy(o => o.Fecha)
                .ToListAsync();

            return View(ordenes);
        }

        // ==========================================================
        // ENTREGAR PEDIDO AL CLIENTE (Mesero finaliza el pedido)
        // ==========================================================
        [HttpPost]
        public async Task<IActionResult> EntregarAlCliente(int id)
        {
            try
            {
                var orden = await _context.Ordenes
                    .Include(o => o.Pedido)
                    .FirstOrDefaultAsync(o => o.IdOrden == id);

                if (orden == null)
                    return Json(new { success = false, message = "Orden no encontrada" });

                if (orden.EstadoCocina != "Entregado")
                    return Json(new { success = false, message = "Esta orden no está lista para entregar" });

                // Cambiar estado del pedido a completado (false = completado)
                orden.Pedido.Estado = false;

                // Eliminar la orden de la tabla (ya fue completada)
                _context.Ordenes.Remove(orden);

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Pedido entregado al cliente exitosamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}
