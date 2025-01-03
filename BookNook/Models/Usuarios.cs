﻿using System.ComponentModel.DataAnnotations;

namespace BookNook.Models
{
    public class Usuarios
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public string Contraseña { get; set; }

        [Required, EmailAddress]
        public string Correo { get; set; }

        public virtual ICollection<ObjetivosLectura> ObjetivosLectura { get; set; }

        public Usuarios()
        {
            ObjetivosLectura = new HashSet<ObjetivosLectura>();
        }
    }
}
