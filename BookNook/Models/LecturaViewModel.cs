using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Models
{
    public class LecturaViewModel
    {
        [Column("libro_id")]
        public int LibroId { get; set; }
        public List<SelectListItem> Libros { get; set; }

        [Column("estado_id")]
        public int EstadoId { get; set; }

        [Column("fecha_inicio")]
        public DateTime? FechaInicio { get; set; }

        [Column("fecha_fin")]
        public DateTime? FechaFin { get; set; }
        public int? Calificacion { get; set; }
        public string? Notas { get; set; }

        [Column("pagina_actual")]
        public int? PaginaActual { get; set; }
    }
}
