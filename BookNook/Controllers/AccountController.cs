using BookNook.Data;
using BookNook.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string encryptedPassword = EncryptPassword(model.Contraseña);
            var user = _context.Usuarios.FirstOrDefault(u =>
                u.Correo == model.Correo &&
                u.Contraseña == encryptedPassword);

            if (user != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Correo)
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Inicio");
            }

            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
            return View(model);
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string encryptedPassword = EncryptPassword(model.Contraseña);
                var user = new Usuarios
                {
                    Correo = model.Correo,
                    Contraseña = encryptedPassword,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                };

                _context.Usuarios.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(model);
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

