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

            var latestReadingsIds = _context.Lecturas
                .Where(l => l.UsuarioId == int.Parse(userId))
                .GroupBy(l => l.LibroId)
                .Select(g => g.OrderByDescending(l => l.FechaInicio)
                             .ThenByDescending(l => l.Id)
                             .First().Id);

            var baseQuery = _context.Lecturas
                .Where(l => latestReadingsIds.Contains(l.Id))
                .Include(l => l.Libro);

            var booksQuery = baseQuery.Select(l => l.Libro).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                booksQuery = booksQuery.Where(b => b.Titulo.Contains(searchTerm) || b.Autor.Contains(searchTerm));
            }

            var lecturasQuery = baseQuery.AsQueryable();

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

            var filteredBookIds = lecturasQuery.Select(l => l.LibroId).ToList();

            if (filter != "Todos")
            {
                booksQuery = booksQuery.Where(b => filteredBookIds.Contains(b.Id));
            }

            var model = booksQuery
                .Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Titulo = b.Titulo ?? "Sin título",
                    Autor = b.Autor ?? "Sin autor",
                    ImagenPortada = b.ImagenPortada ?? "/imagen_libro/predeterminado/predeterminado.jpg",
                    Progreso = baseQuery
                        .Where(l => l.LibroId == b.Id)
                        .Select(l =>
                            l.EstadoId == 1 ? 100 :
                            (int?)((l.PaginaActual ?? 0) * 100 / b.NumeroPaginas))
                        .FirstOrDefault(),
                    FechaInicio = baseQuery
                        .Where(l => l.LibroId == b.Id)
                        .Select(l => l.FechaInicio)
                        .FirstOrDefault(),
                    FechaFin = baseQuery
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
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                    if (string.IsNullOrEmpty(userId))
                    {
                        Console.WriteLine("Error: Usuario no encontrado");
                        TempData["ErrorMessage"] = "Usuario no autenticado";
                        return RedirectToAction("Login", "Account");
                    }

                    var lecturaExistente = await _context.Lecturas
                    .FirstOrDefaultAsync(l => l.UsuarioId == int.Parse(userId) &&
                                              l.LibroId == model.LibroId &&
                                              l.EstadoId == model.EstadoId);

                    if (lecturaExistente != null)
                    {
                        TempData["ErrorMessage"] = "Este libro ya tiene una lectura registrada.";

                        model.Libros = await _context.Libros
                           .Select(l => new SelectListItem
                           {
                               Value = l.Id.ToString(),
                               Text = l.Titulo
                           })
                           .ToListAsync();

                        return View(model);
                    }

                    double? avanceLectura = null; 

                    if (model.EstadoId == 1) 
                    {
                        avanceLectura = 100.0;
                    }
                    else if (model.EstadoId == 3 && model.PaginaActual.HasValue && model.NumeroPaginas > 0) 
                    {
                        double progreso = (model.PaginaActual.Value * 100.0) / (double)model.NumeroPaginas;
                        avanceLectura = Math.Min(progreso, 100.0);
                    }
                    else
                    {
                        avanceLectura = 0.0; 
                    }

                    model.AvanceLectura = avanceLectura;

                    var lectura = new Lecturas
                    {
                        UsuarioId = int.Parse(userId),
                        LibroId = model.LibroId,
                        EstadoId = model.EstadoId,
                        FechaInicio = model.FechaInicio,
                        FechaFin = model.FechaFin,
                        Calificacion = model.Calificacion,
                        Notas = model.Notas,
                        PaginaActual = model.PaginaActual,
                        CreadoEn = DateTime.Now,
                        ActualizadoEn = DateTime.Now
                    };

                    Console.WriteLine($"Intentando agregar lectura: LibroId={lectura.LibroId}, UsuarioId={lectura.UsuarioId}, EstadoId={lectura.EstadoId}");

                    _context.Lecturas.Add(lectura);
                    await _context.SaveChangesAsync();

                    Console.WriteLine("Lectura agregada exitosamente");
                    TempData["SuccessMessage"] = "Lectura agregada exitosamente.";
                    return RedirectToAction("Biblioteca", "Books");
                }
                else
                {
                    Console.WriteLine("ModelState no es válido:");
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            Console.WriteLine($"Error: {error.ErrorMessage}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar la lectura: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                TempData["ErrorMessage"] = "Error al agregar la lectura: " + ex.Message;
            }

            var librosRecarga = await _context.Libros
            .Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.Titulo
            })
            .ToListAsync();

            model.Libros = librosRecarga;
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            Console.WriteLine($"Attempting to edit book with ID: {id}");

            try
            {
                if (id <= 0)
                {
                    Console.WriteLine($"Invalid ID: {id}");
                    return NotFound($"Invalid book ID: {id}");
                }

                var libro = _context.Libros
                    .Where(b => b.Id == id)
                    .Select(b => new
                    {
                        b.Id,
                        b.Titulo,
                        b.Autor,
                        b.ImagenPortada,
                        b.Genero,
                        b.Subgenero,
                        b.Idioma,
                        NumeroPaginas = b.NumeroPaginas ?? 0,
                        AnoPublicacion = b.AnoPublicacion ?? 0
                    })
                    .FirstOrDefault();

                if (libro == null)
                {
                    Console.WriteLine($"No book found with ID: {id}");
                    TempData["ErrorMessage"] = $"No se encontró un libro con ID {id}";
                    return RedirectToAction("Biblioteca");
                }

                var model = new EditBookViewModel
                {
                    LibroId = libro.Id,
                    Titulo = libro.Titulo,
                    Autor = libro.Autor,
                    ImagenPortada = libro.ImagenPortada,
                    Genero = libro.Genero,
                    Subgenero = libro.Subgenero,
                    Idioma = libro.Idioma,
                    NumeroPaginas = libro.NumeroPaginas,
                    AnoPublicacion = libro.AnoPublicacion
                };

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Edit action: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                TempData["ErrorMessage"] = "Ocurrió un error al intentar editar el libro.";
                return RedirectToAction("Biblioteca");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBookViewModel model)
        {
            Console.WriteLine($"Edit POST method called for book ID: {model.LibroId}");
            Console.WriteLine($"ModelState Valid: {ModelState.IsValid}");

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                    }
                }
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var libro = await _context.Libros.FindAsync(model.LibroId);
                    if (libro == null)
                    {
                        Console.WriteLine($"No book found with ID: {model.LibroId}");
                        TempData["ErrorMessage"] = "Libro no encontrado";
                        return RedirectToAction("Biblioteca");
                    }

                    libro.Titulo = model.Titulo;
                    libro.Autor = model.Autor;
                    libro.Genero = model.Genero;
                    libro.Subgenero = model.Subgenero;
                    libro.Idioma = model.Idioma;
                    libro.NumeroPaginas = model.NumeroPaginas;
                    libro.AnoPublicacion = model.AnoPublicacion;

                    if (model.ImagenPortadaFile != null && model.ImagenPortadaFile.Length > 0)
                    {
                        try
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

                            libro.ImagenPortada = $"/imagen_libro/{uniqueFileName}";
                            Console.WriteLine($"Image uploaded: {libro.ImagenPortada}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Image upload error: {ex.Message}");
                            TempData["ErrorMessage"] = "Error al subir la imagen";
                        }
                    }

                    libro.ActualizadoEn = DateTime.Now;

                    _context.Libros.Update(libro);
                    int saveResult = await _context.SaveChangesAsync();

                    Console.WriteLine($"Save changes result: {saveResult} record(s) updated");

                    TempData["SuccessMessage"] = "El libro se ha actualizado con éxito.";
                    return RedirectToAction("Biblioteca", "Books");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Edit POST method: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                TempData["ErrorMessage"] = $"Error al guardar: {ex.Message}";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var libro = await _context.Libros
                .Where(b => b.Id == id)
                .Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Titulo = b.Titulo,
                    Autor = b.Autor,
                    ImagenPortada = b.ImagenPortada,
                    Etiquetas = new List<string> { b.Genero, b.Subgenero }
                })
                .FirstOrDefaultAsync();

            if (libro == null)
            {
                TempData["ErrorMessage"] = "Libro no encontrado.";
                return RedirectToAction(nameof(Biblioteca));
            }

            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Books/DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var libro = await _context.Libros.FindAsync(id);
                if (libro == null)
                {
                    return Json(new { success = false, message = "Libro no encontrado." });
                }

                try
                {
                    var lecturasAsociadas = _context.Lecturas.Where(l => l.LibroId == id);
                    _context.Lecturas.RemoveRange(lecturasAsociadas);

                    if (!string.IsNullOrEmpty(libro.ImagenPortada) &&
                        !libro.ImagenPortada.Contains("predeterminado.jpg"))
                    {
                        var imagePath = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            libro.ImagenPortada.TrimStart('/')
                        );

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    _context.Libros.Remove(libro);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "El libro se ha eliminado correctamente.";
                    return Json(new { success = true, message = "Libro eliminado correctamente" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar el libro o su imagen: {ex.Message}");
                    return Json(new { success = false, message = $"Error al eliminar: {ex.Message}" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general al eliminar el libro: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return Json(new { success = false, message = "Error al procesar la solicitud de eliminación." });
            }
        }

    }
}


