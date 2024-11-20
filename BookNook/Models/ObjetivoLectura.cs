using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class ObjetivoLectura
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El año es obligatorio.")]
        [Range(1900, int.MaxValue, ErrorMessage = "El año debe ser un valor válido.")]
        public int Ano { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El objetivo anual debe ser un valor positivo.")]
        public int ObjetivoAnual { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El objetivo mensual debe ser un valor positivo.")]
        public int ObjetivoMensual { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El progreso anual debe ser un valor positivo.")]
        public int ProgresoAnual { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El progreso mensual debe ser un valor positivo.")]
        public int ProgresoMensual { get; set; }

        public DateTime CreadoEn { get; set; }
        public DateTime ActualizadoEn { get; set; }
    }
}
