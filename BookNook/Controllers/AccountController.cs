using BookNook.Data;
using BookNook.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario == null)
            {
                return NotFound();
            }

            var viewModel = new ProfileViewModel
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo = usuario.Correo
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario == null)
            {
                return NotFound();
            }

            var emailExists = await _context.Usuarios
                .AnyAsync(u => u.Correo == model.Correo && u.Id != userId);

            if (emailExists)
            {
                ModelState.AddModelError("Correo", "Este correo ya está en uso");
                return View("Profile", model);
            }

            usuario.Nombre = model.Nombre;
            usuario.Apellido = model.Apellido;
            usuario.Correo = model.Correo;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Información actualizada correctamente";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ErrorMessage"] = "Error al actualizar la información. Por favor, intenta nuevamente.";
            }

            return RedirectToAction(nameof(Profile));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ProfileViewModel model)
        {
            if (string.IsNullOrEmpty(model.ContraseñaActual) ||
                string.IsNullOrEmpty(model.NuevaContraseña) ||
                string.IsNullOrEmpty(model.ConfirmarContraseña))
            {
                TempData["ErrorMessage"] = "Todos los campos de contraseña son requeridos";
                return RedirectToAction(nameof(Profile));
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var usuario = await _context.Usuarios.FindAsync(userId);

            if (usuario == null)
            {
                return NotFound();
            }

            string actualEncriptada = EncryptPassword(model.ContraseñaActual);
            if (actualEncriptada != usuario.Contraseña)
            {
                TempData["ErrorMessage"] = "La contraseña actual es incorrecta";
                return RedirectToAction(nameof(Profile));
            }

            if (model.NuevaContraseña != model.ConfirmarContraseña)
            {
                TempData["ErrorMessage"] = "Las contraseñas no coinciden";
                return RedirectToAction(nameof(Profile));
            }

            usuario.Contraseña = EncryptPassword(model.NuevaContraseña);

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Contraseña actualizada correctamente";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error al actualizar la contraseña. Por favor, intenta nuevamente.";
            }

            return RedirectToAction(nameof(Profile));
        }
    }
}

