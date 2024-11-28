using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Models
{
    public class LecturaViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un libro")]
        [Display(Name = "Libro")]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado de lectura")]
        [Display(Name = "Estado")]
        [Range(1, 4, ErrorMessage = "El estado seleccionado no es válido")]
        public int EstadoId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de inicio")]
        [PastOrPresentDate(ErrorMessage = "La fecha de inicio no puede ser futura")]
        public DateTime? FechaInicio { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de fin")]
        [PastOrPresentDate(ErrorMessage = "La fecha de fin no puede ser futura")]
        [DateGreaterThan("FechaInicio", ErrorMessage = "La fecha de fin debe ser posterior a la fecha de inicio")]
        public DateTime? FechaFin { get; set; }

        [Range(0, 10, ErrorMessage = "La calificación debe estar entre 0 y 10")]
        [Display(Name = "Calificación")]
        public int? Calificacion { get; set; }

        [StringLength(1000, ErrorMessage = "Las notas no pueden exceder los 1000 caracteres")]
        [Display(Name = "Notas")]
        public string? Notas { get; set; }

        [Display(Name = "Página actual")]
        public int? PaginaActual { get; set; }

        public double? AvanceLectura { get; set; }

        public int? NumeroPaginas { get; set; }

        public IEnumerable<SelectListItem>? Libros { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EstadoId == 3 && (!PaginaActual.HasValue || PaginaActual.Value <= 0))
            {
                yield return new ValidationResult(
                    "Debe ingresar una página actual válida cuando el estado es 'Leyendo'",
                    new[] { nameof(PaginaActual) }
                );
            }

            if (PaginaActual.HasValue && NumeroPaginas.HasValue && PaginaActual.Value > NumeroPaginas.Value)
            {
                yield return new ValidationResult(
                    "La página actual no puede ser mayor al número total de páginas",
                    new[] { nameof(PaginaActual) }
                );
            }
        }
    }

    public class PastOrPresentDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date.Date <= DateTime.Now.Date;
            }
            return true;
        }
    }

    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                throw new ArgumentException("Propiedad no encontrada");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (comparisonValue == null || value is not DateTime dateValue)
                return ValidationResult.Success;

            if (dateValue.Date <= comparisonValue.Value.Date)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}