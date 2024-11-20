using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class ListasLibros
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre de la lista es obligatorio.")]
        [StringLength(255, ErrorMessage = "El nombre de la lista no puede exceder los 255 caracteres.")]
        public string NombreLista { get; set; }

        [StringLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres.")]
        public string Descripcion { get; set; }

        public DateTime CreadoEn { get; set; }
        public DateTime ActualizadoEn { get; set; }
    }
}
