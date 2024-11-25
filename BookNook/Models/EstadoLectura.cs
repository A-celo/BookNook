using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Models
{
    [Table("estado_lectura")]
    public class EstadoLectura
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(255, ErrorMessage = "La descripción no puede exceder los 255 caracteres.")]
        public string Descripcion { get; set; }
    }
}
