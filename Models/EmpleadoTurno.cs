using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class EmpleadoTurno
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEmpleadoTurno { get; set; }

        [Required]
        public DateTime FechaTurno { get; set; }

        // FK Empleado
        public int EmpleadoId { get; set; }
        public Empleado Empleados { get; set; }

        // FK Turno
        public int TurnoId { get; set; }
        public Turno Turnos { get; set; }
    }
}
