using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class LecturaViewModel
    {
        [Required(ErrorMessage = "Debes seleccionar un libro")]
        public int LibroId { get; set; }

        public List<SelectListItem> Libros { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un estado")]
        public int EstadoId { get; set; }

        [Display(Name = "Fecha de inicio")]
        [DataType(DataType.Date)]
        public DateTime? FechaInicio { get; set; }

        [Display(Name = "Fecha de fin")]
        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }

        [Range(0, 10, ErrorMessage = "La calificación debe estar entre 0 y 10")]
        [Display(Name = "Calificación")]
        public int? Calificacion { get; set; }

        [StringLength(1000, ErrorMessage = "Las notas no pueden exceder los 1000 caracteres")]
        public string? Notas { get; set; }

        [Display(Name = "Página actual")]
        [Range(0, int.MaxValue, ErrorMessage = "La página actual debe ser un valor positivo")]
        public int? PaginaActual { get; set; }

        public LecturaViewModel()
        {
            Libros = new List<SelectListItem>();
        }
    }
}