using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class Usuario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Required, StringLength(50)]
        public string NombreUsuario { get; set; }

        [Required, StringLength(255)]
        public string Correo { get; set; }

        [Required, StringLength(30)]
        public string Contrasena { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public bool Estado { get; set; }

        // Relación opcional con Empleado
        public int? EmpleadoId { get; set; }
        public Empleado? Empleado { get; set; }

        // Relación opcional con Cliente
        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        // Relación con Rol
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        [NotMapped]
        public string ConfirmarContrasena { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
    }
}
