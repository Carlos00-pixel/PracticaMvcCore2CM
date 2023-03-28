using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2CM.Models;
using PracticaMvcCore2CM.Repositories;
using System.Diagnostics;

namespace PracticaMvcCore2CM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private RepositoryLibros repo;

        public HomeController(ILogger<HomeController> logger, RepositoryLibros repo)
        {
            _logger = logger;
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Libro> libros = this.repo.GetLibros();
            return View(libros);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}