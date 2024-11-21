using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [StringLength(13, ErrorMessage = "El ISBN debe tener 13 caracteres.")]
        public string Isbn { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(255, ErrorMessage = "El título no puede exceder los 255 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [StringLength(255, ErrorMessage = "El autor no puede exceder los 255 caracteres.")]
        public string Autor { get; set; }

        [Column("ano_publicacion")]
        [Range(1000, 9999, ErrorMessage = "El año de publicación debe ser un valor entre 1000 y 9999.")]
        public int AnoPublicacion { get; set; }

        [StringLength(50, ErrorMessage = "El idioma no puede exceder los 50 caracteres.")]
        public string Idioma { get; set; }

        [StringLength(255, ErrorMessage = "El género no puede exceder los 255 caracteres.")]
        public string Genero { get; set; }

        [StringLength(255, ErrorMessage = "El subgénero no puede exceder los 255 caracteres.")]
        public string Subgenero { get; set; }

        [Column("numero_paginas")]
        [Range(1, int.MaxValue, ErrorMessage = "El número de páginas debe ser un valor positivo.")]
        public int NumeroPaginas { get; set; }

        [Column("duracion_audio")]
        [Range(1, int.MaxValue, ErrorMessage = "La duración del audio debe ser un valor positivo.")]
        public int DuracionAudio { get; set; }

        [Column("imagen_portada")]
        [StringLength(255, ErrorMessage = "La imagen de portada no puede exceder los 255 caracteres.")]
        public string ImagenPortada { get; set; }

        [StringLength(1000, ErrorMessage = "La sinopsis no puede exceder los 1000 caracteres.")]
        public string Sinopsis { get; set; }

        [Column("creado_en")]
        public DateTime CreadoEn { get; set; }

        [Column("actualizado_en")]
        public DateTime ActualizadoEn { get; set; }
    }
}
