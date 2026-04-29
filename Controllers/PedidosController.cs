using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_CPS.Data;
using Proyecto_CPS.Models;
using Proyecto_CPS.ViewModels;

namespace Proyecto_CPS.Controllers
{
    public class PedidosController : Controller
    {
        private readonly AppDBContext _context;

        public PedidosController(AppDBContext context)
        {
            _context = context;
        }

        // ========================================
        // VISTA: MIS PEDIDOS (CLIENTE)
        // ========================================
        public IActionResult MisPedidos()
        {
            var userId = HttpContext.Session.GetInt32("IdUsuario");
            if (userId == null)
                return RedirectToAction("Login", "Acceso");

            // Solo traer pedidos NO CONFIRMADOS (Estado = false) del cliente
            var pedidos = _context.Pedidos
                .Include(p => p.DetalleMenuPedidos)
                .ThenInclude(d => d.Menu)
                .Where(p => p.UsuarioId == userId && p.Estado == false)
                .ToList();

            return View(pedidos);
        }

        // ========================================
        // AGREGAR PLATO AL PEDIDO (CLIENTE)
        // ========================================
        public class PedidoDTO
        {
            public int idMenu { get; set; }
        }

        [HttpPost]
        public IActionResult AgregarPedido([FromBody] PedidoDTO data)
        {
            if (data == null)
                return Json(new { success = false, message = "Datos inválidos" });

            int idMenu = data.idMenu;

            var userId = HttpContext.Session.GetInt32("IdUsuario");
            if (userId == null)
                return Json(new { success = false, message = "Debes iniciar sesión" });

            var menu = _context.Menus.Find(idMenu);
            if (menu == null)
                return Json(new { success = false, message = "El plato no existe" });

            // Buscar pedido NO CONFIRMADO del usuario
            var pedido = _context.Pedidos
                .FirstOrDefault(p => p.UsuarioId == userId && p.Estado == false);

            // Si no existe, crear uno nuevo
            if (pedido == null)
            {
                pedido = new Pedido
                {
                    FechaPedido = DateTime.Now,
                    Estado = false, // No confirmado
                    UsuarioId = userId.Value
                };

                _context.Pedidos.Add(pedido);
                _context.SaveChanges();
            }

            // Verificar si el plato ya existe en el pedido
            var detalleExistente = _context.DetalleMenuPedidos
                .FirstOrDefault(d => d.PedidoId == pedido.IdPedido && d.MenuId == idMenu);

            if (detalleExistente != null)
            {
                // Si ya existe, aumentar la cantidad
                detalleExistente.Cantidad++;
                _context.DetalleMenuPedidos.Update(detalleExistente);
            }
            else
            {
                // Si no existe, crear nuevo detalle
                var detalle = new DetalleMenuPedido
                {
                    PedidoId = pedido.IdPedido,
                    MenuId = idMenu,
                    Cantidad = 1
                };
                _context.DetalleMenuPedidos.Add(detalle);
            }

            _context.SaveChanges();

            return Json(new { success = true, message = "Plato agregado al pedido" });
        }

        // ========================================
        // ELIMINAR PLATO DEL PEDIDO (CLIENTE)
        // ========================================
        [HttpPost]
        public IActionResult EliminarPlato(int idDetalle)
        {
            var detalle = _context.DetalleMenuPedidos.Find(idDetalle);
            if (detalle == null)
                return Json(new { success = false, message = "El plato no existe en el pedido" });

            _context.DetalleMenuPedidos.Remove(detalle);
            _context.SaveChanges();

            return Json(new { success = true, message = "Plato eliminado" });
        }

        // ========================================
        // CONFIRMAR PEDIDO (CLIENTE -> MESERO)
        // ========================================
        [HttpPost]
        public IActionResult ConfirmarPedido(int idPedido, int metodoPagoId)
        {
            try
            {
                var pedido = _context.Pedidos
                    .Include(p => p.DetalleMenuPedidos)
                    .ThenInclude(d => d.Menu)
                    .FirstOrDefault(p => p.IdPedido == idPedido);

                if (pedido == null)
                    return Json(new { success = false, message = "Pedido no encontrado" });

                if (pedido.Estado)
                    return Json(new { success = false, message = "El pedido ya fue confirmado" });

                if (pedido.DetalleMenuPedidos == null || !pedido.DetalleMenuPedidos.Any())
                    return Json(new { success = false, message = "El pedido no tiene platos" });

                // Validar que el método de pago existe
                var metodoPago = _context.MetodosPago.Find(metodoPagoId);
                if (metodoPago == null)
                    return Json(new { success = false, message = "Método de pago no válido" });

                // CAMBIAR ESTADO A TRUE = CONFIRMADO
                pedido.Estado = true;
                _context.Pedidos.Update(pedido);

                // GUARDAR MÉTODO DE PAGO SELECCIONADO
                var pedidoMetodoPago = new PedidoMetodoPago
                {
                    PedidoId = idPedido,
                    MetodoPagoId = metodoPagoId,
                    FechaSeleccion = DateTime.Now
                };
                _context.PedidoMetodoPagos.Add(pedidoMetodoPago);

                _context.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "¡Pedido confirmado y enviado al mesero!"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        // ========================================
        // PEDIDOS ACTIVOS (MESERO)
        // ========================================
        public async Task<IActionResult> Activos()
        {
            // Verificar que sea mesero
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Mesero")
            {
                TempData["Error"] = "No tienes permisos para acceder a esta sección";
                return RedirectToAction("Index", "Home");
            }

            // Traer todos los pedidos CONFIRMADOS (Estado = true)
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.DetalleMenuPedidos)
                    .ThenInclude(d => d.Menu)
                .Where(p => p.Estado == true)
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();

            return View(pedidos);
        }

        // ========================================
        // VER DETALLES DEL PEDIDO (MESERO)
        // ========================================
        public async Task<IActionResult> DetallesPedido(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Mesero")
            {
                TempData["Error"] = "No tienes permisos para acceder a esta sección";
                return RedirectToAction("Index", "Home");
            }

            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.DetalleMenuPedidos)
                    .ThenInclude(d => d.Menu)
                        .ThenInclude(m => m.CategoriaMenus)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
            {
                TempData["Error"] = "Pedido no encontrado";
                return RedirectToAction("Activos");
            }

            return View(pedido);
        }

        // ========================================
        // CREAR PEDIDO (MESERO - desde el panel)
        // ========================================
        public IActionResult CrearPedido()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Mesero")
            {
                TempData["Error"] = "No tienes permisos para acceder a esta sección";
                return RedirectToAction("Index", "Home");
            }

            var listaMenus = _context.Menus
                .Include(m => m.CategoriaMenus)
                .ToList();

            return View(listaMenus);
        }

        // ========================================
        // GUARDAR PEDIDO DEL MESERO
        // ========================================
        [HttpPost]
        public IActionResult GuardarPedidoMesero([FromBody] PedidoMeseroVM datos)
        {
            try
            {
                // Validaciones
                if (datos == null || datos.Items == null || !datos.Items.Any())
                    return Json(new { success = false, message = "El pedido debe tener al menos un plato" });

                if (string.IsNullOrWhiteSpace(datos.NombreCliente) || string.IsNullOrWhiteSpace(datos.ApellidoCliente))
                    return Json(new { success = false, message = "Debe ingresar nombre y apellido del cliente" });

                // Verificar que la mesa existe
                var mesa = _context.Mesas.Find(datos.MesaId);
                if (mesa == null)
                    return Json(new { success = false, message = "La mesa seleccionada no existe" });

                // Obtener el ID del mesero logueado
                var meseroId = HttpContext.Session.GetInt32("IdUsuario");
                if (meseroId == null)
                    return Json(new { success = false, message = "Sesión expirada. Por favor, inicie sesión nuevamente" });

                // Crear el pedido
                var pedido = new Pedido
                {
                    FechaPedido = DateTime.Now,
                    Estado = true, // Confirmado automáticamente (va directo a activos)
                    UsuarioId = meseroId.Value // El mesero que toma el pedido
                };

                _context.Pedidos.Add(pedido);
                _context.SaveChanges();

                // Agregar los detalles del pedido
                foreach (var item in datos.Items)
                {
                    var menu = _context.Menus.Find(item.MenuId);
                    if (menu == null) continue;

                    var detalle = new DetalleMenuPedido
                    {
                        PedidoId = pedido.IdPedido,
                        MenuId = item.MenuId,
                        Cantidad = item.Cantidad
                    };

                    _context.DetalleMenuPedidos.Add(detalle);
                }

                _context.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = $"Pedido creado exitosamente para Mesa {datos.MesaId} - Cliente: {datos.NombreCliente} {datos.ApellidoCliente}"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al guardar: " + ex.Message });
            }
        }

        // ========================================
        // PEDIDOS LISTOS PARA ENTREGAR (MESERO)
        // ========================================
        public IActionResult ListosEntrega()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Mesero")
            {
                TempData["Error"] = "No tienes permisos para acceder a esta sección";
                return RedirectToAction("Index", "Home");
            }

            // Aquí puedes implementar la lógica de pedidos listos
            // Por ahora mostramos los pedidos confirmados
            var pedidos = _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.DetalleMenuPedidos)
                    .ThenInclude(d => d.Menu)
                .Where(p => p.Estado == true)
                .OrderByDescending(p => p.FechaPedido)
                .ToList();

            return View(pedidos);
        }
    }
}
