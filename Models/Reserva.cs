using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class Reserva
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdReserva { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public DateTime Hora { get; set; }

        [Required]
        public bool Estado { get; set; }

        // FK Cliente
        public int ClienteId { get; set; }
        public Cliente Clientes { get; set; }

        // FK Mesa
        public int MesaId { get; set; }
        public Mesa Mesas { get; set; }
    }
}
