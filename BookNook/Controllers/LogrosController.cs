using BookNook.Data;
using BookNook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookNook.Controllers
{
    public class LogrosController : Controller
    {
        private readonly BookNookContext _context;

        public LogrosController(BookNookContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            int userIdInt = int.Parse(userId);

            var lecturas = _context.Lecturas
                .Where(l => l.UsuarioId == userIdInt)
                .Include(l => l.Libro)
                .ToList();

            var logros = ObtenerLogros(lecturas);

            return View(logros);
        }

        private List<LogroViewModel> ObtenerLogros(List<Lecturas> lecturas)
        {
            var logros = new List<LogroViewModel>();

            logros.Add(new LogroViewModel
            {
                Nombre = "Mi Primera Lectura",
                Descripcion = "Registrar una lectura en la página.",
                Completado = lecturas.Any()
            });

            logros.Add(new LogroViewModel
            {
                Nombre = "Explorador de Géneros",
                Descripcion = "Leer 5 géneros diferentes.",
                Completado = lecturas.Select(l => l.Libro.Genero).Distinct().Count() >= 5
            });

            logros.Add(new LogroViewModel
            {
                Nombre = "Lector Diario",
                Descripcion = "Leer todos los días durante una semana.",
                Completado = LecturaDiaria(lecturas)
            });

            logros.Add(new LogroViewModel
            {
                Nombre = "Maratón Literario",
                Descripcion = "Leer un libro en menos de 72 horas.",
                Completado = lecturas.Any(l => l.FechaInicio.HasValue && l.FechaFin.HasValue &&
                                               (l.FechaFin.Value - l.FechaInicio.Value).TotalHours <= 72)
            });

            logros.Add(new LogroViewModel
            {
                Nombre = "Lectura Rápida",
                Descripcion = "Leer más de 200 páginas en un solo día.",
                Completado = lecturas.Any(l => l.Libro.NumeroPaginas.HasValue && l.Libro.NumeroPaginas.Value >= 200 &&
                                               l.FechaInicio.HasValue && l.FechaFin.HasValue &&
                                               (l.FechaInicio.Value.Date == l.FechaFin.Value.Date))
            });

            logros.Add(new LogroViewModel
            {
                Nombre = "Biblioteca Creciente",
                Descripcion = "Registrar 10 libros en tu biblioteca personal.",
                Completado = lecturas.Select(l => l.LibroId).Distinct().Count() >= 10
            });

            logros.Add(new LogroViewModel
            {
                Nombre = "Lector Multilingüe",
                Descripcion = "Leer libros en al menos tres idiomas diferentes.",
                Completado = lecturas.Select(l => l.Libro.Idioma).Distinct().Count() >= 3
            });

            logros.Add(new LogroViewModel
            {
                Nombre = "Super Lector",
                Descripcion = "Leer 50 libros en un año.",
                Completado = lecturas.Count(l => l.FechaFin.HasValue && l.FechaFin.Value.Year == DateTime.Now.Year) >= 50
            });

            logros.Add(new LogroViewModel
            {
                Nombre = "Lector Imparable",
                Descripcion = "Leer 100 libros en un año.",
                Completado = lecturas.Count(l => l.FechaFin.HasValue && l.FechaFin.Value.Year == DateTime.Now.Year) >= 100
            });

            return logros;
        }

        private bool LecturaDiaria(List<Lecturas> lecturas)
        {
            var fechas = lecturas.Where(l => l.FechaInicio.HasValue).Select(l => l.FechaInicio.Value.Date).Distinct().OrderBy(d => d).ToList();
            if (fechas.Count < 7) return false;

            for (int i = 0; i <= fechas.Count - 7; i++)
            {
                if (fechas.Skip(i).Take(7).Select((f, index) => f == fechas[i].AddDays(index)).All(b => b))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
