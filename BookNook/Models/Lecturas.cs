using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Models
{
    public class Lecturas
    {
        public int Id { get; set; }

        [Column("usuario_id")]
        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int UsuarioId { get; set; }

        [Column("libro_id")]
        [Required(ErrorMessage = "El ID del libro es obligatorio.")]
        public int LibroId { get; set; }

        [Column("formato_id")]
        [Required(ErrorMessage = "El ID del formato es obligatorio.")]
        public int FormatoId { get; set; }

        [Column("estado_id")]
        [Required(ErrorMessage = "El ID del estado es obligatorio.")]
        public int EstadoId { get; set; }

        [Column("fecha_inicio")]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime FechaInicio { get; set; }

        [Column("fecha_fin")]
        public DateTime? FechaFin { get; set; }

        [Column("pagina_actual")]
        [Range(0, int.MaxValue, ErrorMessage = "La página actual debe ser un valor positivo.")]
        public int? PaginaActual { get; set; }

        [Column("tiempo_actual")]
        [Range(0, int.MaxValue, ErrorMessage = "El tiempo actual debe ser un valor positivo.")]
        public int? TiempoActual { get; set; }

        [Column("tiempo_lectura")]
        [Range(0, int.MaxValue, ErrorMessage = "El tiempo de lectura debe ser un valor positivo.")]
        public int? TiempoLectura { get; set; }

        [Column("calificacion")]
        [Range(0, 10, ErrorMessage = "La calificación debe estar entre 0 y 10.")]
        public decimal? Calificacion { get; set; }

        [StringLength(1000, ErrorMessage = "Las notas no pueden exceder los 1000 caracteres.")]
        public string Notas { get; set; }

        [Column("creado_en")]
        public DateTime CreadoEn { get; set; }

        [Column("actualizado_en")]
        public DateTime ActualizadoEn { get; set; }

        public Libro Libro { get; set; }
    }
}
