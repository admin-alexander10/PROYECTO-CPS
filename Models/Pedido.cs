using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class Pedido
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPedido { get; set; }

        [Required]
        public DateTime FechaPedido { get; set; }

        [Required]
        public bool Estado { get; set; }

        // FK Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // Relaciones
        public ICollection<DetalleMenuPedido> DetalleMenuPedidos { get; set; }
        public Boleta Boletas { get; set; }

        public ICollection<Orden> Ordenes { get; set; }
        // NUEVA RELACIÓN CON MÉTODO DE PAGO
        public PedidoMetodoPago PedidoMetodoPago { get; set; }
    }
}
