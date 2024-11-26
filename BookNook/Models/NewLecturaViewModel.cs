using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class NewLecturaViewModel
    {
        [Required]
        [Display(Name = "Libro")]
        public int LibroId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public int EstadoId { get; set; }

        [Display(Name = "Fecha de inicio")]
        public DateTime? FechaInicio { get; set; }

        [Display(Name = "Fecha de fin")]
        public DateTime? FechaFin { get; set; }

        [Display(Name = "Página actual")]
        public int? PaginaActual { get; set; }

        [Display(Name = "Calificación")]
        public decimal? Calificacion { get; set; }

        [Display(Name = "Notas")]
        public string? Notas { get; set; }

        public List<SelectListItem> Libros { get; set; }
    }
}
