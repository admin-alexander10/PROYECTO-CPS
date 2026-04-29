using Microsoft.AspNetCore.Mvc;

namespace Proyecto_CPS.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult PanelAdmin()
        {
            return View();
        }
    }
}
