using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class Mesa
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMesa { get; set; }

        [Required]
        public int NumMesa { get; set; }

        [Required]
        public int Capacidad { get; set; }

        // Relaciones
        public ICollection<Reserva> Reservas { get; set; }
    }
}
