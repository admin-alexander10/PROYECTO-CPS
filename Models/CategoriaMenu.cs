using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class CategoriaMenu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCategoria { get; set; }

        [Required, StringLength(100)]
        public string NombreDeCategoria { get; set; }

        [Required, StringLength(255)]
        public string Descripcion { get; set; }

        // Relaciones
        public ICollection<Menu> Menus { get; set; }
    }
}
