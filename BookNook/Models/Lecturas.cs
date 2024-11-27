using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Models
{
    public class Lecturas
    {
        public int Id { get; set; }

        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("libro_id")]
        public int LibroId { get; set; }

        [Column("estado_id")]
        public int EstadoId { get; set; }

        [Column("fecha_inicio")]
        public DateTime? FechaInicio { get; set; }

        [Column("fecha_fin")]
        public DateTime? FechaFin { get; set; }

        [Column("pagina_actual")]
        [Range(0, int.MaxValue, ErrorMessage = "La página actual debe ser un valor positivo.")]
        public int? PaginaActual { get; set; }
        
        [Column("calificacion")]
        [Range(0, 10, ErrorMessage = "La calificación debe estar entre 0 y 10.")]
        public decimal? Calificacion { get; set; }

        [StringLength(1000, ErrorMessage = "Las notas no pueden exceder los 1000 caracteres.")]
        public string? Notas { get; set; }

        [Column("creado_en")]
        public DateTime? CreadoEn { get; set; }

        [Column("actualizado_en")]
        public DateTime? ActualizadoEn { get; set; }

        public Libro Libro { get; set; }
    }
}
