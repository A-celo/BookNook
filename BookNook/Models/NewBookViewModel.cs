using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class NewBookViewModel
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(255, ErrorMessage = "El título no puede exceder los 255 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(255, ErrorMessage = "El autor no puede exceder los 255 caracteres.")]
        public string Autor { get; set; }

        [Display(Name = "Imagen de Portada")]
        public IFormFile? ImagenPortadaFile { get; set; }

        [Range(1000, 9999, ErrorMessage = "El año de publicación debe ser un valor entre 1000 y 9999.")]
        public int AnoPublicacion { get; set; }

        [Required(ErrorMessage = "El género es obligatorio.")]
        [StringLength(255, ErrorMessage = "El género no puede exceder los 255 caracteres.")]
        public string Genero { get; set; }

        [StringLength(255, ErrorMessage = "El subgénero no puede exceder los 255 caracteres.")]
        public string Subgenero { get; set; }

        [StringLength(50, ErrorMessage = "El idioma no puede exceder los 50 caracteres.")]
        public string Idioma { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El número de páginas debe ser un valor positivo.")]
        public int NumeroPaginas { get; set; }
    }
}
