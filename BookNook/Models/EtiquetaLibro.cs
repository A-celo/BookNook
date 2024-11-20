using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class EtiquetaLibro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del libro es obligatorio.")]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "El ID de la etiqueta es obligatorio.")]
        public int EtiquetaId { get; set; }
    }
}
