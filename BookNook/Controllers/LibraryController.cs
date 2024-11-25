using BookNook.Data;
using BookNook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookNook.Controllers
{
    public class LibraryController : Controller
    {
        private readonly BookNookContext _context;

        public LibraryController(BookNookContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var libros = await _context.Libros
            .Select(l => new BookViewModel
            {
                Titulo = l.Titulo ?? "Sin título",
                Autor = l.Autor ?? "Sin autor",
                ImagenPortada = l.ImagenPortada ?? "/imagen_libro/predeterminado/predeterminado.jpg",
                Etiquetas = new List<string>
                {
                    l.Genero ?? "Sin género",
                    l.Subgenero ?? "Sin subgénero"
                }
            })
            .ToListAsync();

            return View("~/Views/Books/Index.cshtml", libros);
        }
    }
}
