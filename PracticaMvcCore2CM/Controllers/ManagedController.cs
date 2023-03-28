using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2CM.Models;
using PracticaMvcCore2CM.Repositories;
using System.Security.Claims;

namespace PracticaMvcCore2CM.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryUsuarios repo;

        public ManagedController(RepositoryUsuarios repo)
        {
            this.repo = repo;
        }

        public IActionResult DetallesUsuario(int idusuario)
        {
            //idusuario = int.Parse(HttpContext.User.FindFirst("Id")?.Value);
            Usuario user = this.repo.FindUsuario(idusuario);
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email
            , string password)
        {
            Usuario usuario =
                await this.repo.ExisteUsuario(email, password);
            if (usuario != null)
            {
                ClaimsIdentity identity =
               new ClaimsIdentity
               (CookieAuthenticationDefaults.AuthenticationScheme
               , ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim
                    (new Claim(ClaimTypes.Name, usuario.Email));
                identity.AddClaim
                    (new Claim(ClaimTypes.NameIdentifier, usuario.Pass.ToString()));
                identity.AddClaim
                    (new Claim("Nombre", usuario.Nombre));
                identity.AddClaim
                    (new Claim("Foto", usuario.Foto.ToString()));
                identity.AddClaim
                    (new Claim("Id", usuario.IdUsuario.ToString()));

                ClaimsPrincipal user = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}
