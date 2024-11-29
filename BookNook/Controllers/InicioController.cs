using BookNook.Data;
using BookNook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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
            var userEmail = User.Identity.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var user = _context.Usuarios.FirstOrDefault(u => u.Correo == userEmail);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var objetivo = _context.ObjetivosLectura
                .FirstOrDefault(o => o.UsuarioId == user.Id && o.Año == DateTime.Now.Year);

            if (objetivo == null)
            {
                objetivo = new ObjetivosLectura
                {
                    UsuarioId = user.Id,
                    Año = DateTime.Now.Year,
                    ObjetivoAnual = 12,
                    ProgresoAnual = 0,
                    LibrosLeidos = 0,
                    LibrosRestantes = 12,
                    ActualizadoEn = DateTime.Now
                };
                _context.ObjetivosLectura.Add(objetivo);
                _context.SaveChanges();
            }

            var librosCompletadosEsteAño = _context.Lecturas
                .Where(l => l.UsuarioId == user.Id &&
                            l.FechaFin.HasValue &&
                            l.FechaFin.Value.Year == DateTime.Now.Year)
                .Count();

            if (objetivo.ProgresoAnual != librosCompletadosEsteAño)
            {
                objetivo.ProgresoAnual = librosCompletadosEsteAño;
                objetivo.LibrosLeidos = librosCompletadosEsteAño;
                objetivo.LibrosRestantes = Math.Max(0, objetivo.ObjetivoAnual - librosCompletadosEsteAño);
                objetivo.ActualizadoEn = DateTime.Now;
                _context.SaveChanges();
            }

            var primerDiaMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var primerDiaSiguienteMes = primerDiaMes.AddMonths(1);

            var lecturasRecientes = _context.Lecturas
                .Where(l => l.UsuarioId == user.Id &&
                            l.FechaInicio.HasValue &&
                            l.FechaInicio.Value >= primerDiaMes &&
                            l.FechaInicio.Value < primerDiaSiguienteMes)
                .OrderByDescending(l => l.FechaInicio)
                .Take(5)
                .Include(l => l.Libro)
                .Select(l => new LecturaRecienteViewModel
                {
                    Titulo = l.Libro != null ? l.Libro.Titulo : "Título no disponible",
                    Autor = l.Libro != null ? l.Libro.Autor : "Autor no disponible",
                    ImagenPortada = l.Libro != null ? l.Libro.ImagenPortada : "",
                    AvanceLectura = l.PaginaActual.HasValue
                        ? (l.PaginaActual.Value * 100 / (l.Libro.NumeroPaginas ?? 1))
                        : (l.FechaFin.HasValue ? 100 : 0),
                    Fecha = l.FechaInicio.HasValue
                        ? l.FechaInicio.Value.ToString("dd/MM/yyyy")
                        : "",
                })
                .ToList();

            var lecturasMensuales = _context.Lecturas
                .Where(l => l.UsuarioId == user.Id &&
                            l.FechaFin.HasValue &&
                            l.FechaFin.Value.Year == DateTime.Now.Year)
                .GroupBy(l => l.FechaFin.Value.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    Cantidad = g.Count()
                })
                .ToList()
                .Select(g => new LecturaMensual
                {
                    Mes = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Mes),
                    Cantidad = g.Cantidad
                })
                .OrderBy(l => DateTime.ParseExact(l.Mes, "MMM", CultureInfo.CurrentCulture).Month)
                .ToList();

            var generos = _context.Lecturas
               .Where(l => l.UsuarioId == user.Id &&
                           (l.FechaFin.HasValue || l.FechaInicio.HasValue) && 
                           (!l.FechaFin.HasValue || l.FechaFin.Value.Year == DateTime.Now.Year)) 
               .Include(l => l.Libro)
               .Where(l => l.Libro.Genero != null)
               .GroupBy(l => l.Libro.Genero)
               .Select(g => new GeneroLectura
               {
                   Nombre = g.Key,
                   Valor = g.Count()
               })
               .OrderByDescending(g => g.Valor)
               .ToList();

            var viewModel = new InicioViewModel
            {
                ObjetivoAnual = objetivo.ObjetivoAnual,
                ProgresoAnual = librosCompletadosEsteAño,
                LecturasRecientes = lecturasRecientes,
                LecturasPorMes = lecturasMensuales,
                GenerosPorcentaje = generos
            };

            return View(viewModel);
        }

        public ActionResult Biblioteca()
        {
            var librosConLecturas = from libro in _context.Libros
                                    join lectura in _context.Lecturas
                                    on libro.Id equals lectura.LibroId into lecturaGroup
                                    from lectura in lecturaGroup.DefaultIfEmpty()
                                    select new LibroConLecturaViewModel
                                    {
                                        Titulo = libro.Titulo ?? "Sin título",
                                        Autor = libro.Autor ?? "Sin autor",
                                        ImagenPortada = libro.ImagenPortada ?? "",
                                        Progreso = lectura != null ?
                                            (lectura.PaginaActual ?? 0) : 0,
                                        FechaInicio = lectura != null ?
                                            lectura.FechaInicio : null,
                                        FechaFin = lectura != null ?
                                            lectura.FechaFin : null
                                    };

            var model = librosConLecturas.ToList();

            return View(model);
        }

    }
}
