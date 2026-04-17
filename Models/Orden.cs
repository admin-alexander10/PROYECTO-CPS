using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class Orden
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdOrden { get; set; }

        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public string EstadoCocina { get; set; }

        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }
    }
}
