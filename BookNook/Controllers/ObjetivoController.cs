using BookNook.Data;
using BookNook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookNook.Controllers
{
    public class ObjetivoController : Controller
    {
        private readonly BookNookContext _context;

        public ObjetivoController(BookNookContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult EstablecerObjetivo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EstablecerObjetivo(int objetivoAnual)
        {
            var userEmail = User.Identity.Name;
            var user = _context.Usuarios.FirstOrDefault(u => u.Correo == userEmail);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var nuevoObjetivo = new ObjetivosLectura
            {
                UsuarioId = user.Id,
                Año = DateTime.Now.Year,
                ObjetivoAnual = objetivoAnual,
                ProgresoAnual = 0,
                CreadoEn = DateTime.Now,
                ActualizadoEn = DateTime.Now,
                LibrosLeidos = 0,
                LibrosRestantes = objetivoAnual
            };

            _context.ObjetivosLectura.Add(nuevoObjetivo);
            _context.SaveChanges();

            return RedirectToAction("Index", "Inicio");
        }
    }
}
