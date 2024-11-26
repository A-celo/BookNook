using BookNook.Data;
using BookNook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookNook.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookNookContext _context;

        public BooksController(BookNookContext context)
        {
            _context = context;
        }

        public IActionResult Biblioteca(string filter = "Todos", string searchTerm = "")
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var booksQuery = _context.Libros.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                booksQuery = booksQuery.Where(b => b.Titulo.Contains(searchTerm) || b.Autor.Contains(searchTerm));
            }

            var lecturasQuery = _context.Lecturas
            .Where(l => l.UsuarioId == int.Parse(userId))
            .Include(l => l.Libro)
            .AsQueryable();

            switch (filter)
            {
                case "PorLeer":
                    lecturasQuery = lecturasQuery.Where(l => l.EstadoId == 2);
                    break;
                case "Leyendo":
                    lecturasQuery = lecturasQuery.Where(l => l.EstadoId == 3);
                    break;
                case "Leidos":
                    lecturasQuery = lecturasQuery.Where(l => l.EstadoId == 1);
                    break;
                case "DNF":
                    lecturasQuery = lecturasQuery.Where(l => l.EstadoId == 5);  
                    break;
                default:  
                    lecturasQuery = lecturasQuery.Where(l => l.EstadoId != 0); 
                    break;
            }

            var filteredBookIds = lecturasQuery.Select(l => l.LibroId).Distinct().ToList();

            if (filter != "Todos")
            {
                booksQuery = booksQuery.Where(b => filteredBookIds.Contains(b.Id));
            }

            var model = booksQuery
            .Select(b => new BookViewModel
            {
                Titulo = b.Titulo ?? "Sin título",
                Autor = b.Autor ?? "Sin autor",
                ImagenPortada = b.ImagenPortada ?? "/imagen_libro/predeterminado/predeterminado.jpg",
                Progreso = lecturasQuery
                    .Where(l => l.LibroId == b.Id)
                    .Select(l => (int?)((l.PaginaActual ?? 0) * 100 / b.NumeroPaginas))
                    .FirstOrDefault(),
                FechaInicio = lecturasQuery
                    .Where(l => l.LibroId == b.Id)
                    .Select(l => l.FechaInicio)
                    .FirstOrDefault(),
                FechaFin = lecturasQuery
                    .Where(l => l.LibroId == b.Id)
                    .Select(l => l.FechaFin)
                    .FirstOrDefault(),
                Etiquetas = new List<string>
                {
                    b.Genero ?? "Sin género",
                    b.Subgenero ?? "Sin subgénero"
                }
            })
            .ToList();

            ViewBag.Filter = filter;
            return View("Index", model);
        }

        public IActionResult Add()
        {
            var model = new NewBookViewModel
            {
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(NewBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                string rutaImagen = null;

                if (model.ImagenPortadaFile != null && model.ImagenPortadaFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagen_libro");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImagenPortadaFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImagenPortadaFile.CopyToAsync(fileStream);
                    }

                    rutaImagen = $"/imagen_libro/{uniqueFileName}";
                }

                rutaImagen ??= "/imagen_libro/predeterminado/predeterminado.jpg";

                var libro = new Libro
                {
                    Titulo = model.Titulo,
                    Autor = model.Autor,
                    ImagenPortada = rutaImagen,
                    Genero = model.Genero,
                    Subgenero = model.Subgenero,
                    Idioma = model.Idioma,
                    NumeroPaginas = model.NumeroPaginas,
                    AnoPublicacion = model.AnoPublicacion,
                    CreadoEn = DateTime.Now,
                    ActualizadoEn = DateTime.Now
                };

                _context.Libros.Add(libro);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "El libro se ha agregado con éxito.";
                return RedirectToAction("Index", "Library");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddLectura()
        {
            var libros = await _context.Libros.Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.Titulo }).ToListAsync();

            var model = new LecturaViewModel
            {
                Libros = libros
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLectura(LecturaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lectura = new Lecturas
                {
                    LibroId = model.LibroId,
                    EstadoId = model.EstadoId,
                    FechaInicio = model.FechaInicio,
                    FechaFin = model.FechaFin,
                    Calificacion = model.Calificacion,
                    Notas = model.Notas,
                    PaginaActual = model.PaginaActual
                };

                _context.Lecturas.Add(lectura);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Lectura agregada exitosamente.";
                return RedirectToAction("Biblioteca", "Books");
            }

            var libros = await _context.Libros
            .Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.Titulo
            })
            .ToListAsync();

            model.Libros = libros;

            return View(model);
        }

    }
}


