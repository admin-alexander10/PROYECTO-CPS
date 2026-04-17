using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class MetedoPago
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMetodoDePago { get; set; }

        [Required, StringLength(50)]
        public string Nombre { get; set; }

        // Relaciones
        public ICollection<DetalleMetodoPago> DetalleMetodoPagos { get; set; }
    }
}
