using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Proyecto_CPS.Data;
using Proyecto_CPS.Models;
using System.IO;
using System.Text;
using System.Xml;

namespace Proyecto_CPS.Controllers
{
    public class ReservaController : Controller
    {
        private readonly AppDBContext _context;

        public ReservaController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Reserva - Mostrar mesas disponibles
        public async Task<IActionResult> Index()
        {
            try
            {
                // Obtener todas las mesas ordenadas por número
                var mesas = await _context.Mesas
                    .OrderBy(m => m.NumMesa)
                    .ToListAsync();

                // Obtener reservas activas (Estado = true) para HOY
                var hoy = DateTime.Now.Date;
                var reservasActivas = await _context.Reservas
                    .Where(r => r.Estado == true && r.Fecha.Date == hoy)
                    .Select(r => r.MesaId)
                    .Distinct()
                    .ToListAsync();

                ViewBag.Mesas = mesas;
                ViewBag.MesasReservadas = reservasActivas;

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar las mesas: " + ex.Message;
                return View();
            }
        }

        // GET: Reserva/MisReservas - Ver reservas del usuario
        public async Task<IActionResult> MisReservas()
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("IdUsuario");

                if (userId == null)
                {
                    TempData["Info"] = "Inicia sesión para ver tus reservas";
                    return RedirectToAction("Login", "Acceso");
                }

                // Buscar el usuario con su cliente asociado
                var usuario = await _context.Usuarios
                    .Include(u => u.Cliente)
                    .FirstOrDefaultAsync(u => u.IdUsuario == userId);

                List<Reserva> reservas = new List<Reserva>();

                if (usuario?.ClienteId != null)
                {
                    // Obtener todas las reservas del cliente (activas y canceladas)
                    reservas = await _context.Reservas
                        .Include(r => r.Mesas)
                        .Include(r => r.Clientes)
                        .Where(r => r.ClienteId == usuario.ClienteId)
                        .OrderByDescending(r => r.Fecha)
                        .ThenByDescending(r => r.Hora)
                        .ToListAsync();
                }

                return View(reservas);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al cargar tus reservas: " + ex.Message;
                return View(new List<Reserva>());
            }
        }

        // POST: Cancelar Reserva
        [HttpPost]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("IdUsuario");
                if (userId == null)
                    return Json(new { success = false, message = "Debes iniciar sesión" });

                // Buscar la reserva con sus relaciones
                var reserva = await _context.Reservas
                    .Include(r => r.Clientes)
                    .Include(r => r.Mesas)
                    .FirstOrDefaultAsync(r => r.IdReserva == id);

                if (reserva == null)
                    return Json(new { success = false, message = "Reserva no encontrada" });

                // Verificar que la reserva pertenece al usuario logueado
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.IdUsuario == userId);

                if (usuario?.ClienteId != reserva.ClienteId)
                    return Json(new { success = false, message = "No tienes permiso para cancelar esta reserva" });

                // Cambiar el estado a false (cancelada)
                reserva.Estado = false;

                _context.Reservas.Update(reserva);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = $"Reserva cancelada exitosamente. La Mesa {reserva.Mesas.NumMesa} ahora está disponible."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al cancelar: " + ex.Message });
            }
        }

        // GET: Reserva/SeleccionarMesa
        public async Task<IActionResult> SeleccionarMesa(int id)
        {
            try
            {
                var mesa = await _context.Mesas.FindAsync(id);

                if (mesa == null)
                {
                    TempData["Error"] = "Mesa no encontrada";
                    return RedirectToAction("Index");
                }

                // Verificar si la mesa ya está reservada para hoy
                var hoy = DateTime.Now.Date;
                var reservaExistente = await _context.Reservas
                    .FirstOrDefaultAsync(r => r.MesaId == id && r.Fecha.Date == hoy && r.Estado == true);

                if (reservaExistente != null)
                {
                    TempData["Error"] = $"La Mesa {mesa.NumMesa} ya está reservada para hoy";
                    return RedirectToAction("Index");
                }

                ViewBag.Mesa = mesa;
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al seleccionar la mesa: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Reserva/Crear
        [HttpPost]
        public async Task<IActionResult> Crear(Reserva reserva, int mesaId)
        {
            try
            {
                // Validar que la fecha no sea en el pasado
                if (reserva.Fecha.Date < DateTime.Now.Date)
                {
                    TempData["Error"] = "No puedes hacer reservas para fechas pasadas";
                    return RedirectToAction("SeleccionarMesa", new { id = mesaId });
                }

                // Verificar si la mesa existe
                var mesa = await _context.Mesas.FindAsync(mesaId);
                if (mesa == null)
                {
                    TempData["Error"] = "La mesa seleccionada no existe";
                    return RedirectToAction("Index");
                }

                // Verificar si la mesa ya está reservada en esa fecha y hora
                var reservaExistente = await _context.Reservas
                    .FirstOrDefaultAsync(r =>
                        r.MesaId == mesaId &&
                        r.Fecha.Date == reserva.Fecha.Date &&
                        r.Hora.Hour == reserva.Hora.Hour &&
                        r.Estado == true);

                if (reservaExistente != null)
                {
                    TempData["Error"] = $"La Mesa {mesa.NumMesa} ya está reservada para {reserva.Fecha.ToString("dd/MM/yyyy")} a las {reserva.Hora.ToString("HH:mm")}";
                    return RedirectToAction("SeleccionarMesa", new { id = mesaId });
                }

                // Obtener el usuario logueado
                var userId = HttpContext.Session.GetInt32("IdUsuario");
                Cliente cliente = null;

                if (userId != null)
                {
                    // Buscar el usuario con su cliente asociado
                    var usuario = await _context.Usuarios
                        .Include(u => u.Cliente)
                        .FirstOrDefaultAsync(u => u.IdUsuario == userId);

                    if (usuario != null && usuario.ClienteId != null)
                    {
                        // Usuario ya tiene cliente asociado
                        cliente = usuario.Cliente;

                        // Actualizar datos del cliente si se proporcionaron nuevos
                        cliente.PrimNombre = reserva.Clientes.PrimNombre;
                        cliente.SegNombre = reserva.Clientes.SegNombre;
                        cliente.ApellidoP = reserva.Clientes.ApellidoP;
                        cliente.ApellidoM = reserva.Clientes.ApellidoM;
                        cliente.Telefono = reserva.Clientes.Telefono;
                        cliente.Correo = reserva.Clientes.Correo;

                        _context.Clientes.Update(cliente);
                        await _context.SaveChangesAsync();
                    }
                    else if (usuario != null)
                    {
                        // Usuario sin cliente: crear cliente y asociarlo
                        cliente = new Cliente
                        {
                            PrimNombre = reserva.Clientes.PrimNombre,
                            SegNombre = reserva.Clientes.SegNombre,
                            ApellidoP = reserva.Clientes.ApellidoP,
                            ApellidoM = reserva.Clientes.ApellidoM,
                            Telefono = reserva.Clientes.Telefono,
                            Correo = reserva.Clientes.Correo
                        };

                        _context.Clientes.Add(cliente);
                        await _context.SaveChangesAsync();

                        // Asociar cliente al usuario
                        usuario.ClienteId = cliente.IdCliente;
                        _context.Usuarios.Update(usuario);
                        await _context.SaveChangesAsync();
                    }
                }

                // Si no hay cliente (no debería pasar si está logueado)
                if (cliente == null)
                {
                    TempData["Error"] = "Error: No se pudo crear el perfil de cliente";
                    return RedirectToAction("SeleccionarMesa", new { id = mesaId });
                }

                // Crear la reserva
                var nuevaReserva = new Reserva
                {
                    ClienteId = cliente.IdCliente,
                    MesaId = mesaId,
                    Fecha = reserva.Fecha.Date,
                    Hora = reserva.Hora,
                    Estado = true // Reserva activa
                };

                _context.Reservas.Add(nuevaReserva);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"¡Reserva creada exitosamente! Mesa {mesa.NumMesa} reservada para el {reserva.Fecha.ToString("dd/MM/yyyy")} a las {reserva.Hora.ToString("HH:mm")}";
                return RedirectToAction("MisReservas");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al crear la reserva: " + ex.Message;
                return RedirectToAction("SeleccionarMesa", new { id = mesaId });
            }
        }

        // GET: Verificar disponibilidad (AJAX)
        [HttpGet]
        public async Task<JsonResult> VerificarDisponibilidad(int mesaId, DateTime fecha, DateTime hora)
        {
            try
            {
                var reservaExistente = await _context.Reservas
                    .FirstOrDefaultAsync(r =>
                        r.MesaId == mesaId &&
                        r.Fecha.Date == fecha.Date &&
                        r.Hora.Hour == hora.Hour &&
                        r.Estado == true);

                var disponible = reservaExistente == null;

                return Json(new
                {
                    disponible = disponible,
                    mensaje = disponible ? "Mesa disponible" : "Mesa no disponible en esta fecha y hora"
                });
            }
            catch (Exception ex)
            {
                return Json(new { disponible = false, mensaje = "Error al verificar disponibilidad: " + ex.Message });
            }
        }


        public IActionResult AdminReservas(DateTime? fecha)
        {
            var reservas = _context.Reservas
                .Include(r => r.Clientes)
                .Include(r => r.Mesas)
                .AsQueryable();

            if (fecha != null)
                reservas = reservas.Where(r => r.Fecha.Date == fecha.Value.Date);

            return View(reservas.ToList());
        }


        [HttpPost]
        public IActionResult EliminarReserva(int id)
        {
            var reserva = _context.Reservas.Find(id);
            if (reserva == null)
            {
                TempData["Error"] = "La reserva no existe.";
                return RedirectToAction("AdminReservas");
            }

            _context.Reservas.Remove(reserva);
            _context.SaveChanges();

            TempData["Success"] = "Reserva eliminada permanentemente.";
            return RedirectToAction("AdminReservas");
        }

        public IActionResult ExportarReservasPDF()
        {
            var reservas = _context.Reservas
                .Include(r => r.Clientes)
                .Include(r => r.Mesas)
                .OrderBy(r => r.Fecha)
                .ThenBy(r => r.Hora)
                .ToList();

            // Crear HTML para el PDF
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<meta charset='utf-8'>");
            html.AppendLine("<title>Reporte de Reservas - Rancho Azul</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine("h1 { color: #0a3c78; text-align: center; }");
            html.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 20px; }");
            html.AppendLine("th { background-color: #0a3c78; color: white; padding: 10px; text-align: left; }");
            html.AppendLine("td { padding: 8px; border-bottom: 1px solid #ddd; }");
            html.AppendLine("tr:nth-child(even) { background-color: #f2f2f2; }");
            html.AppendLine(".badge-activa { background-color: #28a745; color: white; padding: 5px 10px; border-radius: 5px; }");
            html.AppendLine(".badge-cancelada { background-color: #dc3545; color: white; padding: 5px 10px; border-radius: 5px; }");
            html.AppendLine(".footer { text-align: center; margin-top: 30px; color: #666; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            html.AppendLine("<h1>Reporte de Reservas - Rancho Azul</h1>");
            html.AppendLine($"<p style='text-align: center;'>Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}</p>");

            html.AppendLine("<table>");
            html.AppendLine("<thead>");
            html.AppendLine("<tr>");
            html.AppendLine("<th>#</th>");
            html.AppendLine("<th>Cliente</th>");
            html.AppendLine("<th>Mesa</th>");
            html.AppendLine("<th>Fecha</th>");
            html.AppendLine("<th>Hora</th>");
            html.AppendLine("<th>Estado</th>");
            html.AppendLine("</tr>");
            html.AppendLine("</thead>");
            html.AppendLine("<tbody>");

            foreach (var r in reservas)
            {
                html.AppendLine("<tr>");
                html.AppendLine($"<td>{r.IdReserva}</td>");
                html.AppendLine($"<td>{r.Clientes.PrimNombre} {r.Clientes.ApellidoP}</td>");
                html.AppendLine($"<td>Mesa {r.Mesas.NumMesa} (Cap: {r.Mesas.Capacidad})</td>");
                html.AppendLine($"<td>{r.Fecha:dd/MM/yyyy}</td>");
                html.AppendLine($"<td>{r.Hora:HH:mm}</td>");

                if (r.Estado)
                {
                    html.AppendLine("<td><span class='badge-activa'>Activa</span></td>");
                }
                else
                {
                    html.AppendLine("<td><span class='badge-cancelada'>Cancelada</span></td>");
                }

                html.AppendLine("</tr>");
            }

            html.AppendLine("</tbody>");
            html.AppendLine("</table>");

            html.AppendLine("<div class='footer'>");
            html.AppendLine("<p>Rancho Azul - Sistema de Gestión de Reservas</p>");
            html.AppendLine($"<p>Total de reservas: {reservas.Count}</p>");
            html.AppendLine("</div>");

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            // Convertir a bytes
            var bytes = Encoding.UTF8.GetBytes(html.ToString());

            return File(
                bytes,
                "text/html",
                $"Reservas_RanchoAzul_{DateTime.Now:yyyyMMdd_HHmmss}.html"
            );
        }

        private XmlElement CreateNode(XmlDocument doc, string name, string value)
        {
            XmlElement node = doc.CreateElement(name);
            node.InnerText = value;
            return node;
        }
    }
}
