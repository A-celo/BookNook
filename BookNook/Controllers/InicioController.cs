using BookNook.Data;
using BookNook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookNook.Controllers
{
    public class InicioController : Controller
    {
        private readonly BookNookContext _context;

        public InicioController(BookNookContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.Identity.Name; 

            var objetivo = _context.ObjetivosLectura
                .FirstOrDefault(o => o.UsuarioId.ToString() == userId && o.Año == DateTime.Now.Year);

            var lecturasRecientes = _context.Lecturas
             .Where(l => l.UsuarioId.ToString() == userId)
             .OrderByDescending(l => l.FechaInicio)
             .Take(5)
             .Include(l => l.Libro)  // Incluir la relación de Libro
             .Select(l => new LecturaRecienteViewModel
             {
                 Titulo = l.Libro.Titulo,
                 Autor = l.Libro.Autor,
                 AvanceLectura = l.PaginaActual,  // O el campo que indique el avance de lectura
                 Fecha = l.FechaInicio.ToString("dd/MM/yyyy"),
                 ImagenPortada = l.Libro.ImagenPortada
             })
             .ToList();

            var viewModel = new InicioViewModel
            {
                ObjetivoAnual = objetivo?.ObjetivoAnual ?? 0,
                ProgresoAnual = objetivo?.ProgresoAnual ?? 0,
                LecturasRecientes = lecturasRecientes  // Pasamos la lista de lecturas recientes al ViewModel
            };

            return View(viewModel); 
        }


        public IActionResult Biblioteca()
        {
            return View();
        }

        public IActionResult Lecturas()
        {
            return View();
        }

        public IActionResult Logros()
        {
            return View();
        }

        public IActionResult Perfil()
        {
            return View();
        }
    }
}
