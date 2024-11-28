using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BookNook.Models
{
    public class EditBookViewModel
    {
        public int LibroId { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(255, ErrorMessage = "El título no puede exceder los 255 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s\-:,.()]+$",
            ErrorMessage = "El título solo puede contener letras, números y algunos símbolos básicos")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(255, ErrorMessage = "El autor no puede exceder los 255 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-,.]+$",
            ErrorMessage = "El autor solo puede contener letras y algunos símbolos básicos")]
        public string Autor { get; set; }

        [Display(Name = "Imagen de Portada")]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" },
            ErrorMessage = "Solo se permiten archivos de imagen (.jpg, .jpeg, .png, .gif)")]
        [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "El tamaño máximo permitido es 5MB")]
        public IFormFile? ImagenPortadaFile { get; set; }

        public string? ImagenPortada { get; set; }

        [Required(ErrorMessage = "El género es obligatorio")]
        [StringLength(100, ErrorMessage = "El género no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-]+$",
            ErrorMessage = "El género solo puede contener letras")]
        public string Genero { get; set; }

        [StringLength(100, ErrorMessage = "El subgénero no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-]+$",
            ErrorMessage = "El subgénero solo puede contener letras")]
        public string? Subgenero { get; set; }

        [Required(ErrorMessage = "El idioma es obligatorio")]
        [StringLength(50, ErrorMessage = "El idioma no puede exceder los 50 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$",
            ErrorMessage = "El idioma solo puede contener letras")]
        public string Idioma { get; set; }

        [Required(ErrorMessage = "El número de páginas es obligatorio")]
        [Range(1, 10000, ErrorMessage = "El número de páginas debe estar entre 1 y 10000")]
        public int NumeroPaginas { get; set; }

        [Required(ErrorMessage = "El año de publicación es obligatorio")]
        [Range(1000, 2024, ErrorMessage = "El año de publicación debe estar entre 1000 y 2024")]
        public int AnoPublicacion { get; set; }
    }

    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }

    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
