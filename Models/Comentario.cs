using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class Comentario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdComentario { get; set; }

        [Required, StringLength(255)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaComentario { get; set; }

        // FK Cliente
        public int ClienteId { get; set; }
        public Cliente Clientes { get; set; }

        // FK Menu
        public int MenuId { get; set; }
        public Menu Menus { get; set; }
    }
}
