using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ImagenPortada { get; set; }
        public int? Progreso { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public List<string> Etiquetas { get; set; }

        [Column("calificacion")]
        [Range(0, 10, ErrorMessage = "La calificación debe estar entre 0 y 10.")]
        public decimal? Calificacion { get; set; }
    }
}
