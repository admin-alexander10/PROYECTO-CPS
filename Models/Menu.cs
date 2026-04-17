using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class Menu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMenu { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [Required, StringLength(255)]
        public string Descripcion { get; set; }

        [Required, StringLength(255)]
        public string ImagenUrl { get; set; }

        // FK Categoría
        [Required]
        public int CategoriaMenuId { get; set; }
        public CategoriaMenu CategoriaMenus { get; set; }

        // Relaciones
        public ICollection<DetalleMenuPedido> DetalleMenuPedidos { get; set; } = new List<DetalleMenuPedido>();
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
