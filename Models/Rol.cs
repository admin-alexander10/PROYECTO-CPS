using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class Rol
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdRol { get; set; }

        [Required, StringLength(50)]
        public string NombreRol { get; set; }


        // Relaciones
        public ICollection<Empleado> Empleados { get; set; }
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
