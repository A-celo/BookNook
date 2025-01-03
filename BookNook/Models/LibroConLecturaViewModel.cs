﻿namespace BookNook.Models
{
    public class LibroConLecturaViewModel
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ImagenPortada { get; set; }
        public int Progreso { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
