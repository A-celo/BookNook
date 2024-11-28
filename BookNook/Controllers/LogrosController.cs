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

            // Mi Primera Lectura
            var totalLecturas = lecturas.Count;
            logros.Add(new LogroViewModel
            {
                Nombre = "Mi Primera Lectura",
                Descripcion = "Registrar una lectura en la página.",
                Completado = totalLecturas > 0,
                Progreso = totalLecturas > 0 ? 100 : 0,
                ProgresoDetalle = $"{(totalLecturas > 0 ? 1 : 0)}/1 lectura"
            });

            // Explorador de Géneros
            var generosUnicos = lecturas.Select(l => l.Libro.Genero).Distinct().Count();
            var metaGeneros = 5;
            logros.Add(new LogroViewModel
            {
                Nombre = "Explorador de Géneros",
                Descripcion = "Leer 5 géneros diferentes.",
                Completado = generosUnicos >= metaGeneros,
                Progreso = Math.Min((generosUnicos * 100) / metaGeneros, 100),
                ProgresoDetalle = $"{generosUnicos}/{metaGeneros} géneros"
            });

            // Lector Diario
            var diasConsecutivos = CalcularDiasConsecutivos(lecturas);
            var metaDias = 7;
            logros.Add(new LogroViewModel
            {
                Nombre = "Lector Diario",
                Descripcion = "Leer todos los días durante una semana.",
                Completado = diasConsecutivos >= metaDias,
                Progreso = Math.Min((diasConsecutivos * 100) / metaDias, 100),
                ProgresoDetalle = $"{diasConsecutivos}/{metaDias} días"
            });

            // Maratón Literario
            var libroRapido = lecturas.Any(l => l.FechaInicio.HasValue && l.FechaFin.HasValue &&
                                               (l.FechaFin.Value - l.FechaInicio.Value).TotalHours <= 72);
            logros.Add(new LogroViewModel
            {
                Nombre = "Maratón Literario",
                Descripcion = "Leer un libro en menos de 72 horas.",
                Completado = libroRapido,
                Progreso = libroRapido ? 100 : 0,
                ProgresoDetalle = libroRapido ? "¡Completado!" : "0/1 libro"
            });

            // Lectura Rápida
            var lecturasRapidas = lecturas.Count(l => l.Libro.NumeroPaginas.HasValue &&
                                                     l.Libro.NumeroPaginas.Value >= 200 &&
                                                     l.FechaInicio.HasValue && l.FechaFin.HasValue &&
                                                     l.FechaInicio.Value.Date == l.FechaFin.Value.Date);
            logros.Add(new LogroViewModel
            {
                Nombre = "Lectura Rápida",
                Descripcion = "Leer más de 200 páginas en un solo día.",
                Completado = lecturasRapidas > 0,
                Progreso = lecturasRapidas > 0 ? 100 : 0,
                ProgresoDetalle = $"{(lecturasRapidas > 0 ? 1 : 0)}/1 lectura"
            });

            // Biblioteca Creciente
            var librosUnicos = lecturas.Select(l => l.LibroId).Distinct().Count();
            var metaLibros = 10;
            logros.Add(new LogroViewModel
            {
                Nombre = "Biblioteca Creciente",
                Descripcion = "Registrar 10 libros en tu biblioteca personal.",
                Completado = librosUnicos >= metaLibros,
                Progreso = Math.Min((librosUnicos * 100) / metaLibros, 100),
                ProgresoDetalle = $"{librosUnicos}/{metaLibros} libros"
            });

            // Lector Multilingüe
            var idiomasUnicos = lecturas.Select(l => l.Libro.Idioma).Distinct().Count();
            var metaIdiomas = 3;
            logros.Add(new LogroViewModel
            {
                Nombre = "Lector Multilingüe",
                Descripcion = "Leer libros en al menos tres idiomas diferentes.",
                Completado = idiomasUnicos >= metaIdiomas,
                Progreso = Math.Min((idiomasUnicos * 100) / metaIdiomas, 100),
                ProgresoDetalle = $"{idiomasUnicos}/{metaIdiomas} idiomas"
            });

            // Super Lector
            var librosEsteAnio = lecturas.Count(l => l.FechaFin.HasValue &&
                                                    l.FechaFin.Value.Year == DateTime.Now.Year);
            var metaLibrosAnual = 50;
            logros.Add(new LogroViewModel
            {
                Nombre = "Super Lector",
                Descripcion = "Leer 50 libros en un año.",
                Completado = librosEsteAnio >= metaLibrosAnual,
                Progreso = Math.Min((librosEsteAnio * 100) / metaLibrosAnual, 100),
                ProgresoDetalle = $"{librosEsteAnio}/{metaLibrosAnual} libros"
            });

            // Lector Imparable
            var metaLibrosImparable = 100;
            logros.Add(new LogroViewModel
            {
                Nombre = "Lector Imparable",
                Descripcion = "Leer 100 libros en un año.",
                Completado = librosEsteAnio >= metaLibrosImparable,
                Progreso = Math.Min((librosEsteAnio * 100) / metaLibrosImparable, 100),
                ProgresoDetalle = $"{librosEsteAnio}/{metaLibrosImparable} libros"
            });

            return logros;
        }

        private int CalcularDiasConsecutivos(List<Lecturas> lecturas)
        {
            var fechas = lecturas
                .Where(l => l.FechaInicio.HasValue)
                .Select(l => l.FechaInicio.Value.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            if (fechas.Count < 7) return fechas.Count;

            int maxConsecutivos = 1;
            int consecutivosActuales = 1;

            for (int i = 1; i < fechas.Count; i++)
            {
                if (fechas[i] == fechas[i - 1].AddDays(1))
                {
                    consecutivosActuales++;
                    maxConsecutivos = Math.Max(maxConsecutivos, consecutivosActuales);
                }
                else
                {
                    consecutivosActuales = 1;
                }
            }

            return maxConsecutivos;
        }
    }
}
