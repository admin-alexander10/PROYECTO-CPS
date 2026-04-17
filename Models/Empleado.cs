using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class Empleado
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEmpleado { get; set; }

        [Required, StringLength(50)]
        public string PrimNombre { get; set; }

        [StringLength(50)]
        public string SegNombre { get; set; }

        [Required, StringLength(50)]
        public string ApellidoP { get; set; }

        [Required, StringLength(50)]
        public string ApellidoM { get; set; }

        [Required]
        public DateTime FechaContrato { get; set; }

        [Required, StringLength(20)]
        public string Telefono { get; set; }

        [Required, StringLength(100)]
        public string Correo { get; set; }

        // FK Rol
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        // Relaciones
        public ICollection<EmpleadoTurno> EmpleadoTurnos { get; set; }
        public Usuario Usuarios { get; set; }
    }
}
