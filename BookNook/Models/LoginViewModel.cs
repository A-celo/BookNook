using BookNook.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Por favor ingrese su correo electrónico")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Por favor ingrese su contraseña")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [Display(Name = "Contraseña")]
        public string Contraseña { get; set; }
    }
}
