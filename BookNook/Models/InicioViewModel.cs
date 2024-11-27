namespace BookNook.Models
{
    public class InicioViewModel
    {
        public int ObjetivoAnual { get; set; }
        public int ProgresoAnual { get; set; }
        public List<LecturaRecienteViewModel> LecturasRecientes { get; set; }
        public List<LecturaMensual> LecturasPorMes { get; set; }
        public List<GeneroLectura> GenerosPorcentaje { get; set; }
    }

}
