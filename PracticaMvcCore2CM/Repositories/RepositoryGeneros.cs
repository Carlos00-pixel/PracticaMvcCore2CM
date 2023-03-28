using PracticaMvcCore2CM.Data;
using PracticaMvcCore2CM.Models;

namespace PracticaMvcCore2CM.Repositories
{
    public class RepositoryGeneros
    {
        private LibrosContext context;

        public RepositoryGeneros(LibrosContext context)
        {
            this.context = context;
        }

        public List<Genero> GetGeneros()
        {
            return this.context.Generos.ToList();
        }
    }
}
