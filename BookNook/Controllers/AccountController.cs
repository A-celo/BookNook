using Microsoft.AspNetCore.Mvc;
using BookNook.Data;
using BookNook.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BookNook.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookNookContext _context;

        public AccountController(BookNookContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            string encryptedPassword = EncryptPassword(password);

            var user = _context.Usuarios.FirstOrDefault(u => u.Correo == email && u.Contraseña == encryptedPassword);

            if (user != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ErrorMessage = "Email o contraseña incorrectos";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        private string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}

