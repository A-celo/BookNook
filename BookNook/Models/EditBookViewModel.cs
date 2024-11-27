using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class EditBookViewModel
    {
        public int LibroId { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(255, ErrorMessage = "El título no puede exceder los 255 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(255, ErrorMessage = "El autor no puede exceder los 255 caracteres")]
        public string Autor { get; set; }

        [Display(Name = "Imagen de Portada")]
        public IFormFile? ImagenPortadaFile { get; set; }  

        public string? ImagenPortada { get; set; }

        [StringLength(100, ErrorMessage = "El género no puede exceder los 100 caracteres")]
        public string Genero { get; set; }

        [StringLength(100, ErrorMessage = "El subgénero no puede exceder los 100 caracteres")]
        public string Subgenero { get; set; }

        [StringLength(50, ErrorMessage = "El idioma no puede exceder los 50 caracteres")]
        public string Idioma { get; set; }

        [Range(0, 10000, ErrorMessage = "El número de páginas debe estar entre 0 y 10000")]
        public int NumeroPaginas { get; set; }

        [Range(1000, 2100, ErrorMessage = "El año de publicación debe estar entre 1000 y 2100")]
        public int AnoPublicacion { get; set; }
    }
}
