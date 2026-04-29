using Microsoft.AspNetCore.Mvc;
using Proyecto_CPS.Data;

namespace Proyecto_CPS.Controllers
{
    public class MeseroController : Controller
    {
        private readonly AppDBContext _context;

        public MeseroController(AppDBContext context)
        {
            _context = context;
        }

        // ========= PANEL PRINCIPAL =========
        public IActionResult PanelMesero()
        {
            return View();
        }

        // ========= REDIRECCIONES DEL PANEL =========

        // Tomar pedido → PedidoController / CrearPedido
        public IActionResult TomarPedido()
        {
            return RedirectToAction("CrearPedido", "Pedidos");
        }

        // Pedidos activos → PedidoController / Activos
        public IActionResult PedidosActivos()
        {
            return RedirectToAction("Activos", "Pedidos");
        }

        // Pedidos listos para entregar → PedidoController / ListosEntrega
        public IActionResult PedidosListos()
        {
            return RedirectToAction("ListasParaEntregar", "Orden");
        }

        // Ver menú → MenuController / Index
        public IActionResult VerMenu()
        {
            return RedirectToAction("Index", "Menu");
        }
    }
}
