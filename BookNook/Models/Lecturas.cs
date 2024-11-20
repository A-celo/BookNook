using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class Lecturas
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El ID del libro es obligatorio.")]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "El ID del formato es obligatorio.")]
        public int FormatoId { get; set; }

        [Required(ErrorMessage = "El ID del estado es obligatorio.")]
        public int EstadoId { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La página actual debe ser un valor positivo.")]
        public int? PaginaActual { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El tiempo actual debe ser un valor positivo.")]
        public int? TiempoActual { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El tiempo de lectura debe ser un valor positivo.")]
        public int? TiempoLectura { get; set; }

        [Range(0, 10, ErrorMessage = "La calificación debe estar entre 0 y 10.")]
        public decimal? Calificacion { get; set; }

        [StringLength(1000, ErrorMessage = "Las notas no pueden exceder los 1000 caracteres.")]
        public string Notas { get; set; }

        public DateTime CreadoEn { get; set; }
        public DateTime ActualizadoEn { get; set; }
    }
}
