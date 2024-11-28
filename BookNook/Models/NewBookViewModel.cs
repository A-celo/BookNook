using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class NewBookViewModel
    {
        private static readonly string[] AllowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        private const int MaxImageSize = 5 * 1024 * 1024; // 5MB en bytes
        private const int CurrentYear = 2024;

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(255, ErrorMessage = "El título no puede exceder los 255 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9\s\-:,.()]+$",
            ErrorMessage = "El título solo puede contener letras, números y algunos símbolos básicos")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(255, ErrorMessage = "El autor no puede exceder los 255 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-,.]+$",
            ErrorMessage = "El autor solo puede contener letras y algunos símbolos básicos")]
        [Display(Name = "Autor")]
        public string Autor { get; set; }

        [Display(Name = "Imagen de Portada")]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".gif" })]
        [MaxFileSize(MaxImageSize, ErrorMessage = "El tamaño máximo permitido es 5MB")]
        public IFormFile? ImagenPortadaFile { get; set; }

        [Required(ErrorMessage = "El año de publicación es obligatorio")]
        [Range(1000, CurrentYear, ErrorMessage = "El año de publicación debe estar entre 1000 y 2024")]
        [Display(Name = "Año de Publicación")]
        public int AnoPublicacion { get; set; }

        [Required(ErrorMessage = "El género es obligatorio")]
        [StringLength(100, ErrorMessage = "El género no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-]+$",
            ErrorMessage = "El género solo puede contener letras")]
        [Display(Name = "Género")]
        public string Genero { get; set; }

        [StringLength(100, ErrorMessage = "El subgénero no puede exceder los 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-]+$",
            ErrorMessage = "El subgénero solo puede contener letras")]
        [Display(Name = "Subgénero")]
        public string? Subgenero { get; set; }

        [Required(ErrorMessage = "El idioma es obligatorio")]
        [StringLength(50, ErrorMessage = "El idioma no puede exceder los 50 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$",
            ErrorMessage = "El idioma solo puede contener letras")]
        [Display(Name = "Idioma")]
        public string Idioma { get; set; }

        [Required(ErrorMessage = "El número de páginas es obligatorio")]
        [Range(1, 10000, ErrorMessage = "El número de páginas debe estar entre 1 y 10000")]
        [Display(Name = "Número de Páginas")]
        public int NumeroPaginas { get; set; }

        public class AllowedExtensionsAttribute : ValidationAttribute
        {
            private readonly string[] _extensions;

            public AllowedExtensionsAttribute(string[] extensions)
            {
                _extensions = extensions;
                ErrorMessage = $"Solo se permiten archivos de imagen: {string.Join(", ", extensions)}";
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is IFormFile file)
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!_extensions.Contains(extension))
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
                return ValidationResult.Success;
            }
        }

    }
}

