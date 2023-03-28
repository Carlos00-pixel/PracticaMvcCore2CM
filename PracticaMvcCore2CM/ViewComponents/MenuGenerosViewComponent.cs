using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2CM.Models;
using PracticaMvcCore2CM.Repositories;

namespace PracticaMvcCore2CM.ViewComponents
{
    public class MenuGenerosViewComponent: ViewComponent
    {
        private RepositoryGeneros repo;

        public MenuGenerosViewComponent(RepositoryGeneros repo)
        {
            this.repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = this.repo.GetGeneros();
            return View(generos);
        }
    }
}
