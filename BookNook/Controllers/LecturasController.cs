using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookNook.Models;
using BookNook.Data;

public class LecturasController : Controller
{
    private readonly BookNookContext _context;

    public LecturasController(BookNookContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var lecturas = await _context.Lecturas
            .Include(l => l.Libro)
            .ToListAsync();
        return View(lecturas); 
    }

    public IActionResult Create()
    {
        ViewBag.Libros = _context.Libros.Select(l => new { l.Id, l.Titulo, l.Autor, l.Genero }).ToList();
        ViewBag.Estados = _context.EstadoLectura.Select(e => new { e.Id, e.Nombre }).ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Lecturas lectura)
    {
        if (ModelState.IsValid)
        {
            lectura.CreadoEn = DateTime.Now;
            _context.Add(lectura);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        ViewBag.Libros = _context.Libros.Select(l => new { l.Id, l.Titulo, l.Autor, l.Genero }).ToList();
        ViewBag.Estados = _context.EstadoLectura.Select(e => new { e.Id, e.Nombre }).ToList();
        return View(lectura);
    }

}

