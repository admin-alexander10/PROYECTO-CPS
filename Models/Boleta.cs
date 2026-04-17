using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models
{
    public class Boleta
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdBoleta { get; set; }

        [Required, StringLength(50)]
        public string NumBoleta { get; set; }

        [Required]
        public DateTime FechaEmicion { get; set; }

        [Required]
        public decimal Total { get; set; }

        // FK Pedido
        public int PedidoId { get; set; }
        public Pedido Pedidos { get; set; }

        // Relaciones
        public ICollection<Pago> Pagos { get; set; }
    }
}
