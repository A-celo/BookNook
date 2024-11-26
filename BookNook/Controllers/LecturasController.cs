using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookNook.Models;
using BookNook.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookNook.Controllers
{
    public class LecturasController : Controller
    {
        private readonly BookNookContext _context;

        public LecturasController(BookNookContext context)
        {
            _context = context;
        }

        public IActionResult Add()
        {
            var model = new NewLecturaViewModel
            {
                Libros = _context.Libros.Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Titulo
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(NewLecturaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lectura = new Lecturas
                {
                    UsuarioId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value),
                    LibroId = model.LibroId,
                    EstadoId = model.EstadoId,
                    FechaInicio = model.FechaInicio,
                    FechaFin = model.FechaFin,
                    PaginaActual = model.PaginaActual,
                    Calificacion = model.Calificacion,
                    Notas = model.Notas,
                    CreadoEn = DateTime.Now,
                    ActualizadoEn = DateTime.Now
                };

                _context.Lecturas.Add(lectura);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "La lectura se ha agregado con éxito.";
                return RedirectToAction("Biblioteca", "Books");
            }

            model.Libros = _context.Libros.Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.Titulo
            }).ToList();

            return View(model);
        }
    }
}





