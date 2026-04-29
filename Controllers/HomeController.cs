using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto_CPS.Models;

namespace Proyecto_CPS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Si el usuario está logueado, verificar su rol
            var rol = HttpContext.Session.GetString("Rol");

            if (!string.IsNullOrEmpty(rol))
            {
                // Si es mesero, redirigir a su panel
                if (rol == "Mesero")
                {
                    return RedirectToAction("PanelMesero", "Mesero");
                }
                // Si es cocinero, redirigir a su panel
                else if (rol == "Cocinero")
                {
                    return RedirectToAction("PanelCocinero", "Cocinero");
                }
                // Si es administrador, redirigir a su panel
                else if (rol == "Administrador")
                {
                    return RedirectToAction("PanelAdmin", "Admin");
                }
            }

            // Para clientes o usuarios no autenticados, mostrar la página de inicio normal
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
