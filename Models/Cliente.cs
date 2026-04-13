using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_CPS.Models

{
    public class Cliente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdCliente { get; set; }

        [Required, StringLength(50)]
        public string PrimNombre { get; set; }

        [StringLength(50)]
        public string SegNombre { get; set; }

        [Required, StringLength(50)]
        public string ApellidoP { get; set; }

        [Required, StringLength(50)]
        public string ApellidoM { get; set; }

        [Required, StringLength(20)]
        public string Telefono { get; set; }

        [Required, StringLength(100)]
        public string Correo { get; set; }

        // Relaciones
        public ICollection<Reserva>? Reservas { get; set; }
        public ICollection<Comentario>? Comentarios { get; set; }

        // Relación opcional con Usuario
        public int? UsuarioId { get; set; }   // FK opcional
        public Usuario? Usuario { get; set; } // Navegación opcional
    }
}

