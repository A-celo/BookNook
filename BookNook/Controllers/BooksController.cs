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
            var lecturasQuery = _context.Lecturas
                .Include(l => l.Libro)
                .Where(l => l.UsuarioId == int.Parse(userId))
                .AsQueryable();
            switch (filter)
            {
                case "PorLeer":
                    lecturasQuery = lecturasQuery.Where(l => l.EstadoId == 2);
                    break;
                case "Leidos":
                    lecturasQuery = lecturasQuery.Where(l => l.EstadoId == 1);
                    break;
                case "Favoritos":
                    lecturasQuery = lecturasQuery.Where(l => l.Notas.Contains("Favorito"));
                    break;
                case "Wishlist":
                    lecturasQuery = lecturasQuery.Where(l => l.EstadoId == 4);
                    break;
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                lecturasQuery = lecturasQuery.Where(l => l.Libro.Titulo.Contains(searchTerm) || l.Libro.Autor.Contains(searchTerm));
            }
            var model = _context.Lecturas
            .Include(l => l.Libro)
            .Where(l => l.UsuarioId == int.Parse(userId))
            .Select(l => new BookViewModel
            {
                Titulo = l.Libro.Titulo ?? "Sin título",
                Autor = l.Libro.Autor ?? "Sin autor",
                ImagenPortada = l.Libro.ImagenPortada ?? "/imagen_libro/predeterminado/predeterminado.jpg",
                Progreso = l.PaginaActual.HasValue && l.Libro.NumeroPaginas > 0
                    ? (int?)(l.PaginaActual.Value * 100 / l.Libro.NumeroPaginas)
                    : null,
                FechaInicio = l.FechaInicio,
                FechaFin = l.FechaFin,
                Etiquetas = new List<string>
                {
                    l.Libro.Genero ?? "Sin género",
                    l.Libro.Subgenero ?? "Sin subgénero"
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

    }
}


