using BookNook.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookNook.Controllers
{
    public class InicioController : Controller
    {
        private readonly BookNookContext _context;

        // Constructor para inyectar el contexto de la base de datos
        public InicioController(BookNookContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.Identity.Name;  

            var estadisticas = _context.Lecturas
                .Where(l => l.UsuarioId.ToString() == userId)  
                .GroupBy(l => l.FechaInicio.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    LibrosLeidos = g.Count()
                })
                .ToList();

            ViewData["Estadisticas"] = estadisticas;

            return View();
        }

        public IActionResult Biblioteca()
        {
            return View();
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
