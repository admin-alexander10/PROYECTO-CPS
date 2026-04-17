using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class DetalleMenuPedido
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDetallePedido { get; set; }

        [Required]
        public int Cantidad { get; set; }

        // FK Pedido
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }

        // FK Menu
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}
