using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Models
{
    [Table("objetivos_lectura")]
    public class ObjetivosLectura
    {
        public int Id { get; set; }

        [Column("usuario_id")]
        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El año es obligatorio.")]
        [Range(1900, int.MaxValue, ErrorMessage = "El año debe ser un valor válido.")]
        public int Año { get; set; }

        [Column("objetivo_anual")]
        [Range(0, int.MaxValue, ErrorMessage = "El objetivo anual debe ser un valor positivo.")]
        public int ObjetivoAnual { get; set; }

        [Column("progreso_anual")]
        [Range(0, int.MaxValue, ErrorMessage = "El progreso anual debe ser un valor positivo.")]
        public int ProgresoAnual { get; set; }

        [Column("creado_en")]
        public DateTime CreadoEn { get; set; }

        [Column("actualizado_en")]
        public DateTime ActualizadoEn { get; set; }
    }
}
