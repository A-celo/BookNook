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
            .Where(o => o.UsuarioId == user.Id && o.Año == DateTime.Now.Year)
            .Select(o => new
            {
                o.ObjetivoAnual,
                o.ProgresoAnual
            })
            .FirstOrDefault();

            var lecturasRecientes = _context.Lecturas
            .Where(l => l.UsuarioId == user.Id)
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
                Fecha = l.FechaInicio.HasValued
                ? l.FechaInicio.Value.ToString("dd/MM/yyyy")
                : "",
            })

            .ToList();


            var viewModel = new InicioViewModel
            {
                ObjetivoAnual = objetivo?.ObjetivoAnual ?? 0,
                ProgresoAnual = objetivo?.ProgresoAnual ?? 0,
                LecturasRecientes = lecturasRecientes  
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
