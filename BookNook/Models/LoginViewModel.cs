using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Correo electrónico no válido.")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }
    }

}
