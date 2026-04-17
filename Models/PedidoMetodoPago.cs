using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class PedidoMetodoPago
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPedidoMetodoPago { get; set; }

        // FK Pedido
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }

        // FK MetodoPago
        public int MetodoPagoId { get; set; }
        public MetodoPago MetodoPago { get; set; }

        [Required]
        public DateTime FechaSeleccion { get; set; }
    }
}
