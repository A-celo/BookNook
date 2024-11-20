using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class LibroEnLista
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID de la lista es obligatorio.")]
        public int ListaId { get; set; }

        [Required(ErrorMessage = "El ID del libro es obligatorio.")]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "El ID del formato es obligatorio.")]
        public int FormatoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El orden debe ser un valor positivo.")]
        public int Orden { get; set; }

        [StringLength(1000, ErrorMessage = "La nota no puede exceder los 1000 caracteres.")]
        public string Nota { get; set; }

        public DateTime CreadoEn { get; set; }
    }
}
