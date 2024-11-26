namespace BookNook.Models
{
    public class LecturaRecienteViewModel
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string? ImagenPortada { get; set; }
        public int? AvanceLectura { get; set; }
        public int? NumeroPaginas { get; set; }
        public string? Fecha { get; set; }
    }
}
